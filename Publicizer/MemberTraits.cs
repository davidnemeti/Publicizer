using System.Reflection;

namespace Publicizer;

/// <summary>
/// Lifetime of members inside a type.
/// </summary>
public enum MemberLifetime
{
    /// <summary>
    /// Static lifetime.
    /// </summary>
    Static = 1 << 1,

    /// <summary>
    /// Instance lifetime.
    /// </summary>
    Instance = 1 << 2,

    /// <summary>
    /// Represents all lifetimes.
    /// </summary>
    All = Static | Instance
}

/// <summary>
/// Visibility of members inside a type.
/// </summary>
public enum MemberVisibility
{
    /// <summary>
    /// Public visibility.
    /// </summary>
    Public = 1 << 1,

    /// <summary>
    /// Non-public visibility (e.g. private, internal, etc.).
    /// </summary>
    NonPublic = 1 << 2,

    /// <summary>
    /// Represents all visibilities.
    /// </summary>
    All = Public | NonPublic
}

internal static class MemberTraitsConverterExtensions
{
    public static BindingFlags ToBindingFlags(this MemberLifetime memberLifetime)
    {
        var bindingFlags = default(BindingFlags);

        if (memberLifetime.HasFlag(MemberLifetime.Static))
            bindingFlags |= BindingFlags.Static;

        if (memberLifetime.HasFlag(MemberLifetime.Instance))
            bindingFlags |= BindingFlags.Instance;

        return bindingFlags;
    }

    public static BindingFlags ToBindingFlags(this MemberVisibility memberVisibility)
    {
        var bindingFlags = default(BindingFlags);

        if (memberVisibility.HasFlag(MemberVisibility.Public))
            bindingFlags |= BindingFlags.Public;

        if (memberVisibility.HasFlag(MemberVisibility.NonPublic))
            bindingFlags |= BindingFlags.NonPublic;

        return bindingFlags;
    }
}
