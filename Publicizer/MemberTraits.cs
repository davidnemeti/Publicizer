using System.Reflection;

namespace Publicizer;

public enum MemberLifetime
{
    Static = 1 << 1,
    Instance = 1 << 2,
    All = Static | Instance
}

public enum MemberVisibility
{
    Public = 1 << 1,
    NonPublic = 1 << 2,
    All = Public | NonPublic
}

public static class MemberTraitsConverterExtensions
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
