using Publicizer.Runtime;
using Publicizer.Tests.Helpers;
using Publicizer.Tests.Data;
using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForProxyType;

namespace Publicizer.Tests;

public class AccessTestForProxy : AccessTest<ForcedProxy, ForcedProxyStatic>
{
    public AccessTestForProxy()
        : base(instance => new ForcedProxy(instance), () => new ForcedProxyStatic())
    {
    }
}

public class AccessTestForProxyWithCustomMemberAccessor : AccessTest<ForcedProxyWithCustomMemberAccessorType, ForcedProxyWithCustomMemberAccessorTypeStatic>
{
    public AccessTestForProxyWithCustomMemberAccessor()
        : base(instance => new ForcedProxyWithCustomMemberAccessorType(instance), () => new ForcedProxyWithCustomMemberAccessorTypeStatic())
    {
    }
}

[UseStaticLogger]
public abstract class AccessTest<TForcedProxy, TForcedProxyStatic>
    where TForcedProxy : IForcedProxy
    where TForcedProxyStatic : IForcedProxyStatic
{
    protected TypeWithPrivateMembers Instance { get; }
    protected TForcedProxy Proxy { get; }
    protected TForcedProxyStatic StaticProxy { get; }
    protected private ReflectionMemberAccessor ReflectionMemberAccessor { get; }

    protected AccessTest(Func<TypeWithPrivateMembers, TForcedProxy> createProxy, Func<TForcedProxyStatic> createStaticProxy)
    {
        Instance = new();
        Proxy = createProxy(Instance);
        StaticProxy = createStaticProxy();
        ReflectionMemberAccessor = new ReflectionMemberAccessor();
    }

    [Fact]
    public void FieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue< TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._field), 3);

        Assert.Equal(3, Proxy._field);
    }

    [Fact]
    public void FieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._field), 3);

        Proxy._field = 5;

        Assert.Equal(5, Proxy._field);
    }

    [Fact]
    public void ReadonlyFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._readonlyField), 3);

        Assert.Equal(3, Proxy._readonlyField);
    }

    [Fact]
    public void ReadonlyFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._readonlyField), 3);

        Proxy._readonlyField = 5;

        Assert.Equal(5, Proxy._readonlyField);
    }

    [Fact]
    public void ComplexFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._complexField), new OtherType(3));

        Assert.Equal(3, Proxy._complexField.Number);
    }

    [Fact]
    public void ComplexFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._complexField), new OtherType(3));

        Proxy._complexField = new OtherType(5);

        Assert.Equal(5, Proxy._complexField.Number);
    }

    [Fact]
    public void PropertyGet()
    {
        ReflectionMemberAccessor.SetPropertyValue< TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._property), 3);

        Assert.Equal(3, Proxy._property);
    }

    [Fact]
    public void PropertySet()
    {
        ReflectionMemberAccessor.SetPropertyValue<TypeWithPrivateMembers>(Instance, nameof(ForcedProxy._property), 3);

        Proxy._property = 5;

        Assert.Equal(5, Proxy._property);
    }

    [Fact]
    public void ReadonlyPropertyGet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(Instance, GetBackingFieldNameFromPropertyName(nameof(ForcedProxy._readonlyProperty)), 3);

        Assert.Equal(3, Proxy._readonlyProperty);
    }

    [Fact]
    public void ReadonlyPropertySet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(Instance, GetBackingFieldNameFromPropertyName(nameof(ForcedProxy._readonlyProperty)), 3);

        Proxy._readonlyProperty = 5;

        Assert.Equal(5, Proxy._readonlyProperty);
    }

    [Fact]
    public void StaticFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(instance: null, nameof(ForcedProxyStatic.StaticField), 3);

        Assert.Equal(3, StaticProxy.StaticField);
    }

    [Fact]
    public void StaticFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue<TypeWithPrivateMembers>(instance: null, nameof(ForcedProxyStatic.StaticField), 3);

        StaticProxy.StaticField = 5;

        Assert.Equal(5, StaticProxy.StaticField);
    }

    [Fact]
    public void StaticPropertyGet()
    {
        ReflectionMemberAccessor.SetPropertyValue<TypeWithPrivateMembers>(instance: null, nameof(ForcedProxyStatic.StaticProperty), 3);

        Assert.Equal(3, StaticProxy.StaticProperty);
    }

    [Fact]
    public void StaticPropertySet()
    {
        ReflectionMemberAccessor.SetPropertyValue<TypeWithPrivateMembers>(instance: null, nameof(ForcedProxyStatic.StaticProperty), 3);

        StaticProxy.StaticProperty = 5;

        Assert.Equal(5, StaticProxy.StaticProperty);
    }

    [Fact]
    public void StaticProcedureInvocation()
    {
        StaticProxy.StaticProcedure();

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("StaticProcedure", memberName);
        });
    }

    [Fact]
    public void StaticProcedureInvocationWith16Parameters()
    {
        StaticProxy.StaticProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("StaticProcedureWith16Parameters", memberName);
        });
    }

    [Fact]
    public void StaticProcedureInvocationWith17Parameters()
    {
        StaticProxy.StaticProcedureWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("StaticProcedureWith17Parameters", memberName);
        });
    }

    [Fact]
    public void StaticFunctionInvocation()
    {
        var result = StaticProxy.StaticFunction();

        Assert.Equal("hello", result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("StaticFunction", memberName);
        });
    }

    [Fact]
    public void StaticFunctionInvocationWith16Parameters()
    {
        var result = StaticProxy.StaticFunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

        Assert.Equal(136, result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("StaticFunctionWith16Parameters", memberName);
        });
    }

    [Fact]
    public void StaticFunctionInvocationWith17Parameters()
    {
        var result = StaticProxy.StaticFunctionWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

        Assert.Equal(153, result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("StaticFunctionWith17Parameters", memberName);
        });
    }

    [Fact]
    public void ProcedureInvocation()
    {
        Proxy.Procedure();

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("Procedure", memberName);
        });
    }

    [Fact]
    public void ProcedureInvocationWith1Parameter()
    {
        Proxy.Procedure(5);

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("Procedure", memberName);
        });
    }

    [Fact]
    public void ProcedureInvocationWith2Parameters()
    {
        Proxy.Procedure(5, new OtherType(8));

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("Procedure", memberName);
        });
    }

    [Fact]
    public void ProcedureInvocationWith15Parameters()
    {
        Proxy.ProcedureWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("ProcedureWith15Parameters", memberName);
        });
    }

    [Fact]
    public void ProcedureInvocationWith16Parameters()
    {
        Proxy.ProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("ProcedureWith16Parameters", memberName);
        });
    }

    [Fact]
    public void FunctionInvocation()
    {
        var result = Proxy.Function();

        Assert.Equal("hello", result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("Function", memberName);
        });
    }

    [Fact]
    public void FunctionInvocationWith1Parameter()
    {
        var result = Proxy.Function(5);

        Assert.Equal("5", result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("Function", memberName);
        });
    }

    [Fact]
    public void FunctionInvocationWith2Parameters()
    {
        var result = Proxy.Function(5, new OtherType(8));

        Assert.Equal("5, 8", result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("Function", memberName);
        });
    }

    [Fact]
    public void FunctionInvocationWith15Parameters()
    {
        var result = Proxy.FunctionWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

        Assert.Equal(120, result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("FunctionWith15Parameters", memberName);
        });
    }

    [Fact]
    public void FunctionInvocationWith16Parameters()
    {
        var result = Proxy.FunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

        Assert.Equal(136, result);
        Assert.Collection(StaticLogger.LoggedMemberNames, memberName =>
        {
            Assert.Equal("FunctionWith16Parameters", memberName);
        });
    }

    private static string GetBackingFieldNameFromPropertyName(string propertyName) => $"<{propertyName}>k__BackingField";
}
