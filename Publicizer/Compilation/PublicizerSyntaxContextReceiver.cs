using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Publicizer.Annotation;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Publicizer.Compilation;

internal class PublicizerSyntaxContextReceiver : ISyntaxContextReceiver
{
    private List<(INamedTypeSymbol, IImmutableList<AttributeData>)> _proxies = new();

    public IReadOnlyList<(INamedTypeSymbol, IImmutableList<AttributeData>)> Proxies => _proxies;

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is TypeDeclarationSyntax typeDeclarationSyntax &&
            context.SemanticModel.GetDeclaredSymbol(typeDeclarationSyntax) is INamedTypeSymbol namedTypeSymbol &&
            GetPublicizeAttributes(namedTypeSymbol) is var attributeDatas &&
            attributeDatas.Count > 0)
        {
            _proxies.Add((namedTypeSymbol, attributeDatas));
        }
    }

    private IImmutableList<AttributeData> GetPublicizeAttributes(INamedTypeSymbol namedTypeSymbol) =>
        namedTypeSymbol.GetAttributes()
            .Where(IsPublicizeAttribute)
            .ToImmutableArray();

    private bool IsPublicizeAttribute(AttributeData attributeData) =>
        attributeData.AttributeClass != null &&
        attributeData.AttributeClass.Name == typeof(PublicizeAttribute).Name &&
        attributeData.AttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == $"global::{typeof(PublicizeAttribute).FullName}";
}
