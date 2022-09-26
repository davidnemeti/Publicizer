using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Publicizer;

[Generator]
public class PublicizerSourceGenerator : ISourceGenerator
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

            var typeSymbolToPublicize = (INamedTypeSymbol)publicizeAttributeData.ConstructorArguments[0].Value!;
            var memberLifetime = (MemberLifetime)publicizeAttributeData.ConstructorArguments[1].Value!;
            var memberVisibility = (MemberVisibility)publicizeAttributeData.ConstructorArguments[2].Value!;
            var specialMemberAccessorTypeSymbol = (INamedTypeSymbol?) publicizeAttributeData.ConstructorArguments[3].Value;

            if (proxyTypeSymbol.ContainingNamespace is { IsGlobalNamespace: false } @namespace)
            {
                indentedWriter.WriteLine($"namespace {@namespace}");
                indentedWriter.WriteLine("{");
                indentedWriter.Indent++;
                GenerateForwarding(indentedWriter, proxyTypeSymbol, typeSymbolToPublicize, memberLifetime, memberVisibility, specialMemberAccessorTypeSymbol);
                indentedWriter.Indent--;
                indentedWriter.WriteLine("}");
            }
            else
                GenerateForwarding(indentedWriter, proxyTypeSymbol, typeSymbolToPublicize, memberLifetime, memberVisibility, specialMemberAccessorTypeSymbol);

            return stringWriter.ToString();
        }
    }

    private void GenerateForwarding(IndentedTextWriter indentedWriter, INamedTypeSymbol proxyTypeSymbol, INamedTypeSymbol typeSymbolToPublicize, MemberLifetime memberLifetime, MemberVisibility memberVisibility, INamedTypeSymbol? specialMemberAccessorTypeSymbol)
    {
        indentedWriter.WriteLine($"public partial class {proxyTypeSymbol.Name}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;

        // NOTE: The name of the fields are choosen so we can avoid name collisions with the orginial type's members.
        // In case of name collision, one can just simply change the name of the proxy type.
        var instanceToPublicizeName = $"{proxyTypeSymbol.Name}_{typeSymbolToPublicize.Name}";
        var memberAccessorInstanceText = $"{proxyTypeSymbol.Name}_{nameof(IMemberAccessor<object>)}";

        var typeToPublicizeFullName = typeSymbolToPublicize.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var memberAccessorStaticTypeText = $"global::Publicizer.{nameof(IMemberAccessor<object>)}<{typeToPublicizeFullName}>";

        var memberAccessorDynamicTypeText = specialMemberAccessorTypeSymbol != null
            ? specialMemberAccessorTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            : $"global::Publicizer.{nameof(ReflectionMemberAccessor<object>)}<{typeToPublicizeFullName}>";

        var memberAccessorInstantiationText = $"new {memberAccessorDynamicTypeText}()";

        indentedWriter.WriteLine($"private static readonly {memberAccessorStaticTypeText} {memberAccessorInstanceText} = {memberAccessorInstantiationText};");
        indentedWriter.WriteLine();

        if (memberLifetime.HasFlag(MemberLifetime.Instance))
        {
            indentedWriter.WriteLine($"private readonly {typeToPublicizeFullName} {instanceToPublicizeName};");
            indentedWriter.WriteLine();

            const string instanceToPublicizeParameterName = "instanceToPublicize";

            indentedWriter.WriteLine($"public {proxyTypeSymbol.Name}({typeToPublicizeFullName} {instanceToPublicizeParameterName})");
            indentedWriter.WriteLine("{");
            indentedWriter.Indent++;
            indentedWriter.WriteLine($"this.{instanceToPublicizeName} = {instanceToPublicizeParameterName};");
            indentedWriter.Indent--;
            indentedWriter.WriteLine("}");
        }

        var methodSymbolsForProperties = typeSymbolToPublicize
            .GetMembers()
            .OfType<IPropertySymbol>()
            .SelectMany(property => new[] { property.GetMethod, property.SetMethod })
            .ToImmutableHashSet(SymbolEqualityComparer.Default);

        foreach (var memberSymbol in typeSymbolToPublicize.GetMembers())
        {
            if (!MatchMemberLifetime(memberSymbol, memberLifetime) || !MatchMemberVisibility(memberSymbol, memberVisibility))
                continue;

            var instanceText = memberSymbol.IsStatic ? "null" : $"this.{instanceToPublicizeName}";

            switch (memberSymbol)
            {
                case IPropertySymbol propertySymbol:
                    indentedWriter.WriteLine();
                    GenerateProperty(indentedWriter, instanceText, memberAccessorInstanceText, propertySymbol, memberLifetime, memberVisibility);
                    break;

                case IFieldSymbol fieldSymbol when !fieldSymbol.IsImplicitlyDeclared:
                    indentedWriter.WriteLine();
                    GenerateField(indentedWriter, instanceText, memberAccessorInstanceText, fieldSymbol, memberLifetime, memberVisibility);
                    break;

                case IMethodSymbol methodSymbol when !methodSymbol.IsImplicitlyDeclared && !methodSymbolsForProperties.Contains(methodSymbol):
                    indentedWriter.WriteLine();
                    GenerateMethod(indentedWriter, instanceText, memberAccessorInstanceText, methodSymbol, memberLifetime, memberVisibility);
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

    private void GenerateProperty(IndentedTextWriter indentedWriter, string instanceText, string memberAccessorInstanceText, IPropertySymbol propertySymbol, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        var propertyTypeFullName = propertySymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var staticOrEmpty = propertySymbol.IsStatic ? "static " : string.Empty;

        indentedWriter.WriteLine($"public {staticOrEmpty}{propertyTypeFullName} {propertySymbol.Name}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;

        if (propertySymbol.GetMethod is not null)
            GenerateGetSet(indentedWriter, instanceText, memberAccessorInstanceText, propertySymbol, isGet: true, memberLifetime, memberVisibility);

        if (propertySymbol.SetMethod is not null)
            GenerateGetSet(indentedWriter, instanceText, memberAccessorInstanceText, propertySymbol, isGet: false, memberLifetime, memberVisibility);

        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private void GenerateField(IndentedTextWriter indentedWriter, string instanceText, string memberAccessorInstanceText, IFieldSymbol fieldSymbol, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        var fieldTypeFullName = fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var staticOrEmpty = fieldSymbol.IsStatic ? "static " : string.Empty;

        indentedWriter.WriteLine($"public {staticOrEmpty}{fieldTypeFullName} {fieldSymbol.Name}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;
        GenerateGetSet(indentedWriter, instanceText, memberAccessorInstanceText, fieldSymbol, isGet: true, memberLifetime, memberVisibility);
        GenerateGetSet(indentedWriter, instanceText, memberAccessorInstanceText, fieldSymbol, isGet: false, memberLifetime, memberVisibility);
        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private void GenerateMethod(IndentedTextWriter indentedWriter, string instanceText, string memberAccessorInstanceText, IMethodSymbol methodSymbol, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        var returnTypeFullName = methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var staticOrEmpty = methodSymbol.IsStatic ? "static " : string.Empty;

        var parametersSignatureText = string.Join(
            ", ",
            methodSymbol.Parameters.Select(parameter => $"{parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {parameter.Name}")
        );

        var parametersTypeText = string.Join(", ", methodSymbol.Parameters.Select(parameter => $"typeof({parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})"));
        var parametersInvocationText = string.Join(", ", methodSymbol.Parameters.Select(parameter => parameter.Name));

        indentedWriter.WriteLine($"public {staticOrEmpty}{returnTypeFullName} {methodSymbol.Name}({parametersSignatureText}) =>");
        indentedWriter.Indent++;

        if (!methodSymbol.ReturnsVoid)
            indentedWriter.Write($"({returnTypeFullName}) ");

        indentedWriter.WriteLine($"{memberAccessorInstanceText}.InvokeMethod({instanceText}, \"{methodSymbol.Name}\", new Type[] {{ {parametersTypeText} }}, new object[] {{ {parametersInvocationText} }}, global::{typeof(MemberLifetime).FullName}.{memberLifetime}, global::{typeof(MemberVisibility).FullName}.{memberVisibility});");

        indentedWriter.Indent--;
    }

    private void GenerateGetSet(IndentedTextWriter indentedWriter, string instanceText, string memberAccessorInstanceText, ISymbol memberSymbol, bool isGet, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        var memberText = memberSymbol switch
        {
            IPropertySymbol => "Property",
            IFieldSymbol => "Field",
            _ => throw new ArgumentOutOfRangeException(nameof(memberSymbol), memberSymbol, $"Property or field is needed")
        };

        var memberTypeFullName = memberSymbol switch
        {
            IPropertySymbol propertySymbol => propertySymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            IFieldSymbol fieldSymbol => fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            _ => throw new ArgumentOutOfRangeException(nameof(memberSymbol), memberSymbol, $"Property or field is needed")
        };

        if (isGet)
        {
            indentedWriter.WriteLine($"get => {memberAccessorInstanceText}.Get{memberText}Value<{memberTypeFullName}>({instanceText}, \"{memberSymbol.Name}\", global::{typeof(MemberLifetime).FullName}.{memberLifetime}, global::{typeof(MemberVisibility).FullName}.{memberVisibility});");
        }
        else
        {
            indentedWriter.WriteLine($"set => {memberAccessorInstanceText}.Set{memberText}Value<{memberTypeFullName}>({instanceText}, \"{memberSymbol.Name}\", value, global::{typeof(MemberLifetime).FullName}.{memberLifetime}, global::{typeof(MemberVisibility).FullName}.{memberVisibility});");
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new PublicizerSyntaxContextReceiver());
    }
}
