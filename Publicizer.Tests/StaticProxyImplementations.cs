using OuterNamespace.NamespaceForProxyType;

namespace Publicizer.Tests;

public class ForcedProxyStatic : IForcedProxyStatic
{
    public int StaticField { get => ForcedProxy.StaticField; set => ForcedProxy.StaticField = value; }
    public int StaticProperty { get => ForcedProxy.StaticProperty; set => ForcedProxy.StaticProperty = value; }

    public string StaticFunction() => ForcedProxy.StaticFunction();
    public void StaticProcedure() => ForcedProxy.StaticProcedure();
}

public class ForcedProxyWithCustomMemberAccessorTypeStatic : IForcedProxyStatic
{
    public int StaticField { get => ForcedProxyWithCustomMemberAccessorType.StaticField; set => ForcedProxyWithCustomMemberAccessorType.StaticField = value; }
    public int StaticProperty { get => ForcedProxyWithCustomMemberAccessorType.StaticProperty; set => ForcedProxyWithCustomMemberAccessorType.StaticProperty = value; }

    public string StaticFunction() => ForcedProxyWithCustomMemberAccessorType.StaticFunction();
    public void StaticProcedure() => ForcedProxyWithCustomMemberAccessorType.StaticProcedure();
}
