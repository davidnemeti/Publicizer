using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Publicizer.Annotation;
using Publicizer.Runtime;

namespace Publicizer.Compilation;

[Generator]
internal class PublicizerSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var publicizerSyntaxContextReceiver = (PublicizerSyntaxContextReceiver)context.SyntaxContextReceiver!;

        foreach (var (proxyTypeSymbol, publicizeAttributeDatas) in publicizerSyntaxContextReceiver.Proxies)
        {
            var publicizeAttributeData = publicizeAttributeDatas.Single();

            context.AddSource($"{proxyTypeSymbol.Name}.g.cs", SourceText.From(GenerateSource(proxyTypeSymbol, publicizeAttributeData), Encoding.UTF8));
        }
    }

    private string GenerateSource(INamedTypeSymbol proxyTypeSymbol, AttributeData publicizeAttributeData)
    {
        using (var stringWriter = new StringWriter())
        using (var indentedWriter = new IndentedTextWriter(stringWriter))
        {
            indentedWriter.WriteLine("#nullable enable annotations");
            indentedWriter.WriteLine("#nullable disable warnings");

            var typeSymbolToPublicize = (INamedTypeSymbol)publicizeAttributeData.ConstructorArguments[0].Value!;
            var memberLifetime = publicizeAttributeData.GetPublicizeAttributeNamedArgumentValue(nameof(PublicizeAttribute.MemberLifetime), PublicizeAttribute.DefaultMemberLifetime);
            var memberVisibility = publicizeAttributeData.GetPublicizeAttributeNamedArgumentValue(nameof(PublicizeAttribute.MemberVisibility), PublicizeAttribute.DefaultMemberVisibility);
            var accessorHandling = publicizeAttributeData.GetPublicizeAttributeNamedArgumentValue(nameof(PublicizeAttribute.AccessorHandling), PublicizeAttribute.DefaultAccessorHandling);
            var customMemberAccessorTypeSymbol = publicizeAttributeData.GetPublicizeAttributeNamedArgumentValue<INamedTypeSymbol>(nameof(PublicizeAttribute.CustomMemberAccessorType));

            if (proxyTypeSymbol.ContainingNamespace is { IsGlobalNamespace: false } @namespace)
            {
                indentedWriter.WriteLine($"namespace {@namespace}");
                indentedWriter.WriteLine("{");
                indentedWriter.Indent++;
                GenerateForwarding(indentedWriter, proxyTypeSymbol, typeSymbolToPublicize, memberLifetime, memberVisibility, accessorHandling, customMemberAccessorTypeSymbol);
                indentedWriter.Indent--;
                indentedWriter.WriteLine("}");
            }
            else
                GenerateForwarding(indentedWriter, proxyTypeSymbol, typeSymbolToPublicize, memberLifetime, memberVisibility, accessorHandling, customMemberAccessorTypeSymbol);

            return stringWriter.ToString();
        }
    }

    private void GenerateForwarding(IndentedTextWriter indentedWriter, INamedTypeSymbol proxyTypeSymbol, INamedTypeSymbol typeSymbolToPublicize, MemberLifetime memberLifetime, MemberVisibility memberVisibility, AccessorHandling accessorHandling, INamedTypeSymbol? customMemberAccessorTypeSymbol)
    {
        // NOTE: The name of the fields are choosen so we can avoid name collisions with the original type's members.
        // In case of name collision, one can just simply change the name of the proxy type.
        var namer = new Namer(proxyTypeSymbol, typeSymbolToPublicize, customMemberAccessorTypeSymbol, memberLifetime);

        indentedWriter.WriteLine($"/// <summary>");
        indentedWriter.WriteLine($"""/// Proxy type which can be used to access the private members of <see cref="{namer.TypeToPublicizeFullName}" />.""");

        if (memberLifetime.HasFlag(MemberLifetime.Static))
            indentedWriter.WriteLine($"/// Private static members can be accessed through public static members of the proxy type.");

        if (memberLifetime.HasFlag(MemberLifetime.Instance))
            indentedWriter.WriteLine($"/// Private instance members can be accessed through public instance members of the proxy type, thus the proxy type needs to be instantiated.");

        indentedWriter.WriteMultiLine($$"""
            /// </summary>
            public partial class {{proxyTypeSymbol.Name}}
            {
            """);
        indentedWriter.Indent++;

        if (customMemberAccessorTypeSymbol is not null)
        {
            indentedWriter.WriteLine($"private static readonly {namer.MemberAccessorTypeText} {namer.MemberAccessorInstanceText} = new {namer.MemberAccessorTypeText}();");
            indentedWriter.WriteLine();
        }

        if (memberLifetime.HasFlag(MemberLifetime.Instance))
        {
            indentedWriter.WriteLine($"private readonly {namer.TypeToPublicizeFullName} {namer.InstanceToPublicizeName};");
            indentedWriter.WriteLine();

            const string instanceToPublicizeParameterName = "instanceToPublicize";

            indentedWriter.WriteMultiLine($$"""
                /// <summary>
                /// Creates a proxy instance which can be used to access the private instance members of <paramref name="{{instanceToPublicizeParameterName}}" />.
                /// </summary>
                /// <param name="{{instanceToPublicizeParameterName}}">The instance of which private members needs to be accessed.</param>
                public {{proxyTypeSymbol.Name}}({{namer.TypeToPublicizeFullName}} {{instanceToPublicizeParameterName}})
                {
                    this.{{namer.InstanceToPublicizeName}} = {{instanceToPublicizeParameterName}};
                }
                """);
        }

        var methodSymbolsForProperties = typeSymbolToPublicize
            .GetMembers()
            .OfType<IPropertySymbol>()
            .SelectMany(property => new[] { property.GetMethod, property.SetMethod })
            .ToImmutableHashSet(SymbolEqualityComparer.Default);

        var propertyToBackingField = typeSymbolToPublicize
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Where(field => field.AssociatedSymbol is IPropertySymbol)
            .ToImmutableDictionary(field => (IPropertySymbol)field.AssociatedSymbol!, (IEqualityComparer<IPropertySymbol>)SymbolEqualityComparer.Default);

        foreach (var memberSymbol in typeSymbolToPublicize.GetMembers())
        {
            if (!MatchMemberLifetime(memberSymbol, memberLifetime) || !MatchMemberVisibility(memberSymbol, memberVisibility))
                continue;

            switch (memberSymbol)
            {
                case IPropertySymbol propertySymbol:
                    indentedWriter.WriteLine();
                    GenerateProperty(indentedWriter, namer, new Value.Property(propertySymbol, propertyToBackingField), accessorHandling);
                    break;

                case IFieldSymbol fieldSymbol when !fieldSymbol.IsImplicitlyDeclared:
                    indentedWriter.WriteLine();
                    GenerateField(indentedWriter, namer, new Value.Field(fieldSymbol), accessorHandling);
                    break;

                case IMethodSymbol methodSymbol when !methodSymbol.IsImplicitlyDeclared && !methodSymbolsForProperties.Contains(methodSymbol):
                    indentedWriter.WriteLine();
                    GenerateMethod(indentedWriter, namer, new Method(methodSymbol));
                    break;
            }
        }

        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private static bool MatchMemberLifetime(ISymbol memberSymbol, MemberLifetime memberLifetime) =>
        memberLifetime.HasFlag(MemberLifetime.Static) && memberSymbol.IsStatic ||
        memberLifetime.HasFlag(MemberLifetime.Instance) && !memberSymbol.IsStatic;

    private static bool MatchMemberVisibility(ISymbol memberSymbol, MemberVisibility memberVisibility) =>
        memberVisibility.HasFlag(MemberVisibility.Public) && memberSymbol.DeclaredAccessibility == Accessibility.Public ||
        memberVisibility.HasFlag(MemberVisibility.NonPublic) && memberSymbol.DeclaredAccessibility != Accessibility.Public;

    private void GenerateProperty(IndentedTextWriter indentedWriter, Namer namer, Value.Property property, AccessorHandling accessorHandling)
    {
        var staticOrEmpty = property.IsStatic ? "static " : string.Empty;

        Value? valueToRead = property.CanRead || accessorHandling.HasFlag(AccessorHandling.ForceReadOnWriteonly)
            ? property.CanRead ? property : property.BackingField
            : null;

        Value? valueToWrite = property.CanWrite || accessorHandling.HasFlag(AccessorHandling.ForceWriteOnReadonly)
            ? property.CanWrite ? property : property.BackingField
            : null;

        if (valueToRead is not null)
        {
            GenerateMemberInfo(indentedWriter, namer, valueToRead);
            GenerateDelegateIfNeeded(indentedWriter, namer, valueToRead, isGet: true);
        }

        if (valueToWrite is not null)
        {
            if (valueToWrite != valueToRead)
                GenerateMemberInfo(indentedWriter, namer, valueToWrite);

            GenerateDelegateIfNeeded(indentedWriter, namer, valueToWrite, isGet: false);
        }

        indentedWriter.WriteMultiLine($$"""
            /// <summary>
            /// Forwards to property <see cref="{{property.FullName}}" />.
            /// </summary>
            public {{staticOrEmpty}}{{property.TypeFullName}} {{property.Name}}
            {
            """);
        indentedWriter.Indent++;

        if (valueToRead is not null)
            GenerateGetSet(indentedWriter, namer, valueToRead, isGet: true);

        if (valueToWrite is not null)
            GenerateGetSet(indentedWriter, namer, valueToWrite, isGet: false);

        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private void GenerateField(IndentedTextWriter indentedWriter, Namer namer, Value.Field field, AccessorHandling accessorHandling)
    {
        var staticOrEmpty = field.IsStatic ? "static " : string.Empty;
        var shouldWrite = field.CanWrite || accessorHandling.HasFlag(AccessorHandling.ForceWriteOnReadonly);

        GenerateMemberInfo(indentedWriter, namer, field);
        GenerateDelegateIfNeeded(indentedWriter, namer, field, isGet: true);

        if (shouldWrite)
            GenerateDelegateIfNeeded(indentedWriter, namer, field, isGet: false);

        indentedWriter.WriteMultiLine($$"""
            /// <summary>
            /// Forwards to field <see cref="{{field.FullName}}" />.
            /// </summary>
            public {{staticOrEmpty}}{{field.TypeFullName}} {{field.Name}}
            {
            """);
        indentedWriter.Indent++;

        GenerateGetSet(indentedWriter, namer, field, isGet: true);

        if (shouldWrite)
            GenerateGetSet(indentedWriter, namer, field, isGet: false);

        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private void GenerateMethod(IndentedTextWriter indentedWriter, Namer namer, Method method)
    {
        var staticOrEmpty = method.IsStatic ? "static " : string.Empty;

        GenerateMemberInfo(indentedWriter, namer, method);
        GenerateDelegateIfNeeded(indentedWriter, namer, method, out var useTypelessDelegate);

        var parameterNames = method.GetParameterNames().ToImmutableList();
        var parameterTypesFullNames = method.GetParameterTypesFullNames().ToImmutableArray();

        var parametersSignatureText = string.Join(", ", parameterTypesFullNames.Zip(parameterNames, (parameterTypeFullName, parameterName) => $"{parameterTypeFullName} {parameterName}"));
        var parametersTypeOfText = string.Join(", ", parameterTypesFullNames.Select(parameterTypeFullName => $"typeof({parameterTypeFullName})"));
        var parametersTypeXmlDocText = string.Join(", ", parameterTypesFullNames);

        indentedWriter.WriteMultiLine($"""
            /// <summary>
            /// Forwards to method <see cref="{method.FullName}({parametersTypeXmlDocText})"/>.
            /// </summary>
            public {staticOrEmpty}{method.ReturnTypeFullName} {method.Name}({parametersSignatureText}) =>
            """);
        indentedWriter.Indent++;

        if (namer.MemberAccessorInstanceText is null)
        {
            var parametersInvocation = parameterNames;

            if (!method.IsStatic)
                parametersInvocation = parametersInvocation.Insert(0, namer.GetInstanceText(method));

            var parametersInvocationText = string.Join(", ", parametersInvocation);

            if (useTypelessDelegate && !method.Symbol.ReturnsVoid)
                indentedWriter.Write($"({method.ReturnTypeFullName}) ");
            indentedWriter.Write($"{namer.GetInvokeName(method)}");
            if (useTypelessDelegate)
                indentedWriter.Write($".{nameof(Delegate.DynamicInvoke)}");
            indentedWriter.WriteLine($"({parametersInvocationText});");
        }
        else
        {
            var parametersInvocationText = string.Join(", ", parameterNames);

            if (!method.Symbol.ReturnsVoid)
                indentedWriter.Write($"({method.ReturnTypeFullName}) ");

            indentedWriter.WriteMultiLine($$"""
                {{namer.MemberAccessorInstanceText}}.InvokeMethod({{namer.GetMethodInfoName(method)}}, {{namer.GetInstanceText(method)}}, new object[] { {{parametersInvocationText}} });
                """);
        }

        indentedWriter.Indent--;
    }

    private void GenerateMemberInfo(IndentedTextWriter indentedWriter, Namer namer, Value value)
    {
        var memberText = value.Select(field => "Field", property => "Property");
        var valueInfoName = namer.GetValueInfoName(value);
        const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        indentedWriter.WriteMultiLine($"""
            private static readonly global::System.Reflection.{memberText}Info {valueInfoName} = typeof({value.ContainingTypeFullName}).Get{memberText}("{value.Name}", (global::{typeof(BindingFlags).FullName}){GetAsText(bindingFlags)});

            """);
    }

    private void GenerateDelegateIfNeeded(IndentedTextWriter indentedWriter, Namer namer, Value value, bool isGet)
    {
        if (namer.MemberAccessorInstanceText is null)
        {
            var valueInfoName = namer.GetValueInfoName(value);

            var genericParametersText = value.IsStatic
                ? value.TypeFullName
                : $"{value.ContainingTypeFullName}, {value.TypeFullName}";

            if (isGet)
            {
                var getterTypeFullName = $"global::System.Func<{genericParametersText}>";

                indentedWriter.WriteMultiLine($"""
                    private static readonly {getterTypeFullName} {namer.GetValueGetterName(value)} = global::{typeof(MemberInfoHelpers).FullName}.{nameof(MemberInfoHelpers.CreateGetFuncByExpression)}<{getterTypeFullName}>({valueInfoName});

                    """);
            }
            else
            {
                var createSetActionMethodName = value.CanWrite
                    ? $"global::{typeof(MemberInfoHelpers).FullName}.{nameof(MemberInfoHelpers.CreateSetActionByExpression)}"
                    : $"global::Publicizer.Runtime.MemberInfoHelpersContent.CreateSetActionByEmittingIL";

                var setterTypeFullName = $"global::System.Action<{genericParametersText}>";

                indentedWriter.WriteMultiLine($"""
                    private static readonly {setterTypeFullName} {namer.GetValueSetterName(value)} = {createSetActionMethodName}<{setterTypeFullName}>({valueInfoName});

                    """);
            }
        }
    }

    private void GenerateMemberInfo(IndentedTextWriter indentedWriter, Namer namer, Method method)
    {
        var methodInfoName = namer.GetMethodInfoName(method);
        var parametersTypeOfText = string.Join(", ", method.GetParameterTypesFullNames().Select(parameterTypeFullName => $"typeof({parameterTypeFullName})"));
        const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        indentedWriter.WriteMultiLine($$"""
            private static readonly global::{{typeof(MethodInfo).FullName}} {{methodInfoName}} = typeof({{method.ContainingTypeFullName}}).GetMethod("{{method.Name}}", (global::{{typeof(BindingFlags).FullName}}){{GetAsText(bindingFlags)}}, binder: null, new global::{{typeof(Type).FullName}}[] { {{parametersTypeOfText}} }, modifiers: null);

            """);
    }

    private void GenerateDelegateIfNeeded(IndentedTextWriter indentedWriter, Namer namer, Method method, out bool useTypelessDelegate)
    {
        if (namer.MemberAccessorInstanceText is null)
        {
            var methodInfoName = namer.GetMethodInfoName(method);
            var genericParameterTypes = method.GetParameterTypesFullNames().ToList();

            if (!method.IsStatic)
                genericParameterTypes.Insert(0, method.ContainingTypeFullName);

            useTypelessDelegate = genericParameterTypes.Count > 16;

            if (!method.ReturnsVoid)
                genericParameterTypes.Add(method.ReturnTypeFullName);

            var invokeTypeFullName = useTypelessDelegate
                ? $"global::{typeof(Delegate).FullName}"
                : $"global::System.{(method.ReturnsVoid ? "Action" : "Func")}" +
                    (genericParameterTypes.Count > 0
                        ? $"<{string.Join(", ", genericParameterTypes)}>"
                        : string.Empty);

            indentedWriter.WriteMultiLine($"""
                private static readonly {invokeTypeFullName} {namer.GetInvokeName(method)} = global::{typeof(MemberInfoHelpers).FullName}.{nameof(MemberInfoHelpers.CreateInvokeByExpression)}<{invokeTypeFullName}>({methodInfoName});

                """);
        }
        else
            useTypelessDelegate = false;
    }

    private void GenerateGetSet(IndentedTextWriter indentedWriter, Namer namer, Value value, bool isGet)
    {
        var memberText = value.Select(field => "Field", property => "Property");

        if (namer.MemberAccessorInstanceText is null)
        {
            if (isGet)
            {
                indentedWriter.Write($"get => {namer.GetValueGetterName(value)}(");

                if (!value.IsStatic)
                    indentedWriter.Write($"{namer.GetInstanceText(value)}");

                indentedWriter.WriteLine($");");
            }
            else
            {
                indentedWriter.Write($"set => {namer.GetValueSetterName(value)}(");

                if (!value.IsStatic)
                    indentedWriter.Write($"{namer.GetInstanceText(value)}, ");

                indentedWriter.WriteLine($"value);");
            }
        }
        else
        {
            if (isGet)
                indentedWriter.WriteLine($"get => ({value.TypeFullName}) {namer.MemberAccessorInstanceText}.GetValue({namer.GetValueInfoName(value)}, {namer.GetInstanceText(value)});");
            else
                indentedWriter.WriteLine($"set => {namer.MemberAccessorInstanceText}.SetValue({namer.GetValueInfoName(value)}, {namer.GetInstanceText(value)}, value);");
        }
    }

    private string GetAsText(BindingFlags bindingFlags)
    {
        var primitiveTexts = bindingFlags
            .GetFlagsValues()
            .Select(primitiveBindingFlags => $"global::{typeof(BindingFlags).FullName}.{primitiveBindingFlags}");

        return string.Join(" | ", primitiveTexts);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new PublicizerSyntaxContextReceiver());
    }
}
