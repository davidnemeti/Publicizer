using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace Publicizer;

[Generator]
public class PublicizerSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var publicizerSyntaxContextReceiver = (PublicizerSyntaxContextReceiver)context.SyntaxContextReceiver;

        foreach (var (forwarderTypeSymbol, forwarderAttributeDatas) in publicizerSyntaxContextReceiver.Forwarders)
        {
            var forwarderAttributeData = forwarderAttributeDatas.Single();

            context.AddSource($"{forwarderTypeSymbol.Name}.g.cs", SourceText.From(GenerateSource(forwarderTypeSymbol, forwarderAttributeData), Encoding.UTF8));
        }
    }

    private string GenerateSource(INamedTypeSymbol forwarderTypeSymbol, AttributeData forwarderAttributeData)
    {
        using (var stringWriter = new StringWriter())
        using (var indentedWriter = new IndentedTextWriter(stringWriter))
        {
            var typeSymbolToPublicize = (INamedTypeSymbol)forwarderAttributeData.ConstructorArguments[0].Value;
            var generationKind = (GenerationKind)forwarderAttributeData.ConstructorArguments[1].Value;

            if (forwarderTypeSymbol.ContainingNamespace is { IsGlobalNamespace: false } @namespace)
            {
                indentedWriter.WriteLine($"namespace {@namespace}");
                indentedWriter.WriteLine("{");
                indentedWriter.Indent++;
                GenerateForwarding(indentedWriter, forwarderTypeSymbol, typeSymbolToPublicize, generationKind);
                indentedWriter.Indent--;
                indentedWriter.WriteLine("}");
            }
            else
                GenerateForwarding(indentedWriter, forwarderTypeSymbol, typeSymbolToPublicize, generationKind);

            return stringWriter.ToString();
        }
    }

    private void GenerateForwarding(IndentedTextWriter indentedWriter, INamedTypeSymbol forwarderTypeSymbol, INamedTypeSymbol typeSymbolToPublicize, GenerationKind generationKind)
    {
        indentedWriter.WriteLine($"public partial class {forwarderTypeSymbol.Name}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;

        var instanceName = typeSymbolToPublicize.Name;
        var typeFullNameToPublicize = typeSymbolToPublicize.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (generationKind == GenerationKind.Instance)
        {
            indentedWriter.WriteLine($"private readonly {typeFullNameToPublicize} {instanceName};");
            indentedWriter.WriteLine();

            indentedWriter.WriteLine($"public {forwarderTypeSymbol.Name}({typeFullNameToPublicize} {instanceName})");
            indentedWriter.WriteLine("{");
            indentedWriter.Indent++;
            indentedWriter.WriteLine($"this.{instanceName} = {instanceName};");
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
            var generateStatic = generationKind == GenerationKind.Static;
            if (generateStatic != memberSymbol.IsStatic)
                continue;

            var instanceText = memberSymbol.IsStatic ? "null" : $"this.{instanceName}";

            switch (memberSymbol)
            {
                case IPropertySymbol propertySymbol:
                    indentedWriter.WriteLine();
                    GenerateProperty(indentedWriter, instanceText, typeFullNameToPublicize, propertySymbol);
                    break;

                case IFieldSymbol fieldSymbol when !fieldSymbol.IsImplicitlyDeclared:
                    indentedWriter.WriteLine();
                    GenerateField(indentedWriter, instanceText, typeFullNameToPublicize, fieldSymbol);
                    break;

                case IMethodSymbol methodSymbol when !methodSymbol.IsImplicitlyDeclared && !methodSymbolsForProperties.Contains(methodSymbol):
                    indentedWriter.WriteLine();
                    GenerateMethod(indentedWriter, instanceText, typeFullNameToPublicize, methodSymbol);
                    break;
            }
        }

        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private void GenerateProperty(IndentedTextWriter indentedWriter, string instanceText, string typeFullNameToPublicize, IPropertySymbol propertySymbol)
    {
        var propertyTypeFullName = propertySymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var staticOrEmpty = propertySymbol.IsStatic ? "static " : string.Empty;

        indentedWriter.WriteLine($"public {staticOrEmpty}{propertyTypeFullName} {propertySymbol.Name}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;

        if (propertySymbol.GetMethod is not null)
            GenerateGetSet(indentedWriter, instanceText, typeFullNameToPublicize, propertySymbol, isGet: true);

        if (propertySymbol.SetMethod is not null)
            GenerateGetSet(indentedWriter, instanceText, typeFullNameToPublicize, propertySymbol, isGet: false);

        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private void GenerateField(IndentedTextWriter indentedWriter, string instanceText, string typeFullNameToPublicize, IFieldSymbol fieldSymbol)
    {
        var fieldTypeFullName = fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var staticOrEmpty = fieldSymbol.IsStatic ? "static " : string.Empty;

        indentedWriter.WriteLine($"public {staticOrEmpty}{fieldTypeFullName} {fieldSymbol.Name}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;
        GenerateGetSet(indentedWriter, instanceText, typeFullNameToPublicize, fieldSymbol, isGet: true);
        GenerateGetSet(indentedWriter, instanceText, typeFullNameToPublicize, fieldSymbol, isGet: false);
        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
    }

    private void GenerateMethod(IndentedTextWriter indentedWriter, string instanceText, string typeFullNameToPublicize, IMethodSymbol methodSymbol)
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

        indentedWriter.WriteLine($"typeof({typeFullNameToPublicize}).GetMethod(\"{methodSymbol.Name}\", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static, new Type[] {{ {parametersTypeText} }}).Invoke({instanceText}, new object[] {{ {parametersInvocationText} }});");

        indentedWriter.Indent--;
    }

    private void GenerateGetSet(IndentedTextWriter indentedWriter, string instanceText, string typeFullNameToPublicize, ISymbol memberSymbol, bool isGet)
    {
        var getMemberText = memberSymbol switch
        {
            IPropertySymbol => "GetProperty",
            IFieldSymbol => "GetField",
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
            indentedWriter.WriteLine($"get => ({memberTypeFullName}) typeof({typeFullNameToPublicize}).{getMemberText}(\"{memberSymbol.Name}\", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static).GetValue({instanceText});");
        }
        else
        {
            indentedWriter.WriteLine($"set => typeof({typeFullNameToPublicize}).{getMemberText}(\"{memberSymbol.Name}\", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static).SetValue({instanceText}, value);");
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new PublicizerSyntaxContextReceiver());
    }
}
