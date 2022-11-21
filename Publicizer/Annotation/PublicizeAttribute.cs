// NOTE: This file will be included in the receiver project as source code, so we disable nullable warning context when used from the receiver project,
// because nullable behavior changes too frequently between different .NET versions, and we do not want this code to fail at compile time due to nullable problems.
#if !NULLABLE_CHECK_FOR_INCLUDED_CODE
#nullable enable annotations
#nullable disable warnings
#endif

using Publicizer.Runtime;
using System;

namespace Publicizer.Annotation;

/// <summary>
/// Generates public members into the decorated proxy class which forward to the private members of <see cref="TypeToPublicize"/>.
/// This way the private members of <see cref="TypeToPublicize"/> can be accessed with compile-time safety.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal class PublicizeAttribute : Attribute
{
    internal const MemberLifetime DefaultMemberLifetime = MemberLifetime.All;
    internal const MemberVisibility DefaultMemberVisibility = MemberVisibility.All;
    internal const AccessorHandling DefaultAccessorHandling = AccessorHandling.KeepOriginal;

    /// <summary>
    /// The type of which private members need to be accessed.
    /// </summary>
    public Type TypeToPublicize { get; }

    /// <summary>
    /// The lifetime of the members which needs to be generated and forwarded.
    /// </summary>
    public MemberLifetime MemberLifetime { get; set; } = DefaultMemberLifetime;

    /// <summary>
    /// The visibility of the members which needs to be generated and forwarded.
    /// </summary>
    public MemberVisibility MemberVisibility { get; set; } = DefaultMemberVisibility;

    /// <summary>
    /// The handling of generated accessors for fields (readonly vs. read/write) and for properties (<c>get</c> and <c>set</c> accessors).
    /// </summary>
    public AccessorHandling AccessorHandling { get; set; } = DefaultAccessorHandling;

    /// <summary>
    /// Optional member accessor type which implements <see cref="IMemberAccessor{T}"/> where the <c>T</c> generic parameter is <see cref="TypeToPublicize"/>.
    /// If missing, then the default <see cref="ReflectionMemberAccessor{T}"/> will be used.
    /// </summary>
    public Type? CustomMemberAccessorType { get; set; }

    /// <summary>
    /// Generates public members into the decorated proxy class which forward to the private members of <paramref name="typeToPublicize"/>.
    /// </summary>
    /// <param name="typeToPublicize">The type of which private members need to be accessed.</param>
    public PublicizeAttribute(Type typeToPublicize)
    {
        TypeToPublicize = typeToPublicize;
    }
}
