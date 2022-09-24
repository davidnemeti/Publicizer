using System;

namespace Publicizer;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class PublicizeAttribute : Attribute
{
    public Type TypeToPublicize { get; }
    public MemberLifetime MemberLifetime { get; }
    public MemberVisibility MemberVisibility { get; }

    public PublicizeAttribute(Type typeToPublicize, MemberLifetime memberLifetime = MemberLifetime.All, MemberVisibility memberVisibility = MemberVisibility.All)
    {
        TypeToPublicize = typeToPublicize;
        MemberLifetime = memberLifetime;
        MemberVisibility = memberVisibility;
    }
}
