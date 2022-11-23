using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Publicizer.Tests.AmbientScoping;

namespace Publicizer.Tests.Helpers;

public class StaticLogger : AmbientScope<StaticLogger>
{
    private readonly List<string> _loggedMemberNames;

    public static IReadOnlyList<string> LoggedMemberNames => CurrentNotNull._loggedMemberNames;

    public StaticLogger()
    {
        _loggedMemberNames = ParentScope?._loggedMemberNames ?? new();
    }

    public static void Log([CallerMemberName] string memberName = "")
    {
        CurrentNotNull._loggedMemberNames.Add(memberName);
    }

    protected static StaticLogger CurrentNotNull => Current ?? throw new InvalidOperationException($"No active {nameof(StaticLogger)} scope");
}
