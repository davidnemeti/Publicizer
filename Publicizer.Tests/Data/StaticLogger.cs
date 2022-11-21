using System.Diagnostics;
using System.Reflection;
using Architect.AmbientContexts;

namespace NamespaceForTypeWithPrivateMembers;

public class StaticLogger : AmbientScope<StaticLogger>
{
    private readonly List<MethodBase> _loggedMethods;

    public static IReadOnlyList<MethodBase> LoggedMethods => Current._loggedMethods;

    public StaticLogger()
        : base(AmbientScopeOption.JoinExisting)
    {
        _loggedMethods = EffectiveParentScope?._loggedMethods ?? new();
        Activate();
    }

    public static void Log()
    {
        var methodToLog = new StackTrace().GetFrame(1)!.GetMethod()!;
        Current._loggedMethods.Add(methodToLog);
    }

    protected override void DisposeImplementation()
    {
    }

    protected static StaticLogger Current => GetAmbientScope() ?? throw new InvalidOperationException($"No active {nameof(StaticLogger)} scope");
}
