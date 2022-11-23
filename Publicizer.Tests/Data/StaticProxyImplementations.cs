using OuterNamespace.NamespaceForProxyType;

namespace Publicizer.Tests.Data;

public class ForcedProxyStatic : IForcedProxyStatic
{
    public int StaticField { get => ForcedProxy.StaticField; set => ForcedProxy.StaticField = value; }
    public int StaticProperty { get => ForcedProxy.StaticProperty; set => ForcedProxy.StaticProperty = value; }

    public string StaticFunction() => ForcedProxy.StaticFunction();
    public void StaticProcedure() => ForcedProxy.StaticProcedure();

    public int StaticFunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16) =>
        ForcedProxy.StaticFunctionWith16Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);

    public int StaticFunctionWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17) =>
        ForcedProxy.StaticFunctionWith17Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17);

    public void StaticProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16) =>
        ForcedProxy.StaticProcedureWith16Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);

    public void StaticProcedureWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17) =>
        ForcedProxy.StaticProcedureWith17Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17);
}

public class ForcedProxyWithCustomMemberAccessorTypeStatic : IForcedProxyStatic
{
    public int StaticField { get => ForcedProxyWithCustomMemberAccessorType.StaticField; set => ForcedProxyWithCustomMemberAccessorType.StaticField = value; }
    public int StaticProperty { get => ForcedProxyWithCustomMemberAccessorType.StaticProperty; set => ForcedProxyWithCustomMemberAccessorType.StaticProperty = value; }

    public string StaticFunction() => ForcedProxyWithCustomMemberAccessorType.StaticFunction();
    public void StaticProcedure() => ForcedProxyWithCustomMemberAccessorType.StaticProcedure();

    public int StaticFunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16) =>
        ForcedProxyWithCustomMemberAccessorType.StaticFunctionWith16Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);

    public int StaticFunctionWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17) =>
        ForcedProxyWithCustomMemberAccessorType.StaticFunctionWith17Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17);

    public void StaticProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16) =>
        ForcedProxyWithCustomMemberAccessorType.StaticProcedureWith16Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);

    public void StaticProcedureWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17) =>
        ForcedProxyWithCustomMemberAccessorType.StaticProcedureWith17Parameters(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17);
}
