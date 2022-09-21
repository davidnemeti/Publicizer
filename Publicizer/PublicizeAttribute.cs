using System;

namespace Publicizer;

public enum GenerationKind
{
    Static,
    Instance
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class PublicizeAttribute : Attribute
{
    public Type TypeToPublicize { get; }
    public GenerationKind GenerationKind { get; }

    public PublicizeAttribute(Type typeToPublicize, GenerationKind generationKind)
    {
        TypeToPublicize = typeToPublicize;
        GenerationKind = generationKind;
    }
}
