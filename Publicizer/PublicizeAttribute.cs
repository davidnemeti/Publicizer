using System;

namespace Publicizer;

/// <summary>
/// Generates public members into the decorated proxy class which forward to the private members of <see cref="TypeToPublicize"/>.
/// This way the private members of <see cref="TypeToPublicize"/> can be accessed with compile-time safety.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class PublicizeAttribute : Attribute
{
    /// <summary>
    /// The type of which private members need to be accessed.
    /// </summary>
    public Type TypeToPublicize { get; }

    /// <summary>
    /// The lifetime of the members which needs to be generated and forwarded.
    /// </summary>
    public MemberLifetime MemberLifetime { get; }

    /// <summary>
    /// The visibility of the members which needs to be generated and forwarded.
    /// </summary>
    public MemberVisibility MemberVisibility { get; }

    /// <summary>
    /// Optional member accessor type which implements <see cref="IMemberAccessor{T}"/> where the <c>T</c> generic parameter is <see cref="TypeToPublicize"/>.
    /// If missing, then the default <see cref="ReflectionMemberAccessor{T}"/> will be used.
    /// </summary>
    public Type? CustomMemberAccessorType { get; }

    /// <summary>
    /// Generates public members into the decorated proxy class which forward to the private members of <paramref name="typeToPublicize"/>.
    /// </summary>
    /// <param name="typeToPublicize">The type of which private members need to be accessed.</param>
    /// <param name="memberLifetime">The lifetime of the members which needs to be generated and forwarded.</param>
    /// <param name="memberVisibility">The visibility of the members which needs to be generated and forwarded.</param>
    /// <param name="customMemberAccessorType">
    /// Optional member accessor type which implements <see cref="IMemberAccessor{T}"/> where the <c>T</c> generic parameter is <paramref name="typeToPublicize"/>.
    /// If missing, then the default <see cref="ReflectionMemberAccessor{T}"/> will be used.
    /// </param>
    public PublicizeAttribute(Type typeToPublicize, MemberLifetime memberLifetime = MemberLifetime.All, MemberVisibility memberVisibility = MemberVisibility.All, Type? customMemberAccessorType = null)
    {
        TypeToPublicize = typeToPublicize;
        MemberLifetime = memberLifetime;
        MemberVisibility = memberVisibility;
        CustomMemberAccessorType = customMemberAccessorType;
    }
}
