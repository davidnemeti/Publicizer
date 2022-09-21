using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Publicizer;

public class PublicizerSyntaxContextReceiver : ISyntaxContextReceiver
{
    private List<(INamedTypeSymbol, IReadOnlyList<AttributeData>)> _forwarders = new ();

    public IReadOnlyList<(INamedTypeSymbol, IReadOnlyList<AttributeData>)> Forwarders => _forwarders;

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is TypeDeclarationSyntax typeDeclarationSyntax &&
            context.SemanticModel.GetDeclaredSymbol(typeDeclarationSyntax) is INamedTypeSymbol namedTypeSymbol &&
            GetPublicizeAttributes(namedTypeSymbol) is var attributeDatas &&
            attributeDatas.Count > 0)
        {
            _forwarders.Add((namedTypeSymbol, attributeDatas));
        }
    }

    private IReadOnlyList<AttributeData> GetPublicizeAttributes(INamedTypeSymbol namedTypeSymbol) =>
        namedTypeSymbol.GetAttributes()
            .Where(IsPublicizeAttribute)
            .ToArray();

    private bool IsPublicizeAttribute(AttributeData attributeData) =>
        attributeData.AttributeClass.Name == typeof(PublicizeAttribute).Name &&
        attributeData.AttributeClass.ContainingNamespace.Name == typeof(PublicizeAttribute).Namespace;
}
