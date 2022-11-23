using System.Diagnostics;
using System.Reflection;
using Publicizer.Tests.AmbientScoping;

namespace Publicizer.Tests.Helpers;

public class StaticLogger : AmbientScope<StaticLogger>
{
    private readonly List<MethodBase> _loggedMethods;

    public static IReadOnlyList<MethodBase> LoggedMethods => CurrentNotNull._loggedMethods;

    public StaticLogger()
    {
        _loggedMethods = ParentScope?._loggedMethods ?? new();
    }

    public static void Log()
    {
        var methodToLog = new StackTrace().GetFrame(1)!.GetMethod()!;
        CurrentNotNull._loggedMethods.Add(methodToLog);
    }

    protected static StaticLogger CurrentNotNull => Current ?? throw new InvalidOperationException($"No active {nameof(StaticLogger)} scope");
}
