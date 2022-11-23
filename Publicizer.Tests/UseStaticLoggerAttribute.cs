using System.Reflection;
using Xunit.Sdk;
using Publicizer.Tests.Helpers;

namespace Publicizer.Tests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class UseStaticLoggerAttribute : BeforeAfterTestAttribute
{
    private IDisposable? _staticLogger;

    public override void Before(MethodInfo methodUnderTest)
    {
        _staticLogger = new StaticLogger();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        _staticLogger!.Dispose();
    }
}
