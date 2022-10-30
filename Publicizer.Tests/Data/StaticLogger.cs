using System.Diagnostics;
using System.Reflection;

namespace NamespaceForTypeWithPrivateMembers;

public static class StaticLogger
{
    private readonly static List<MethodBase> _loggedMethods = new ();

    public static IReadOnlyList<MethodBase> LoggedMethods => _loggedMethods;

    public static void Log()
    {
        var methodToLog = new StackTrace().GetFrame(1)!.GetMethod()!;
        _loggedMethods.Add(methodToLog);
    }

    public static void Reset()
    {
        _loggedMethods.Clear();
    }
}
