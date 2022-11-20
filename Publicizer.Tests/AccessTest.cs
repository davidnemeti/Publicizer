using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForProxyType;

namespace Publicizer.Tests;

public class AccessTestForProxy : AccessTest<ForcedProxy>
{
    public AccessTestForProxy()
        : base(instance => new ForcedProxy(instance))
    {
    }
}

public class AccessTestForProxyWithCustomMemberAccessor : AccessTest<ForcedProxyWithCustomMemberAccessorType>
{
    public AccessTestForProxyWithCustomMemberAccessor()
        : base(instance => new ForcedProxyWithCustomMemberAccessorType(instance))
    {
    }
}

[UseStaticLogger]
public abstract class AccessTest<TForcedProxy>
    where TForcedProxy : IForcedProxy
{
    protected TypeWithPrivateMembers Instance { get; }
    protected IForcedProxy Proxy { get; }
    protected ReflectionMemberAccessor<TypeWithPrivateMembers> ReflectionMemberAccessor { get; }

    protected AccessTest(Func<TypeWithPrivateMembers, TForcedProxy> createProxy)
    {
        Instance = new();
        Proxy = createProxy(Instance);
        ReflectionMemberAccessor = new ReflectionMemberAccessor<TypeWithPrivateMembers>();
    }

    [Fact]
    public void FieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(ForcedProxy._field), 3);

        Assert.Equal(3, Proxy._field);
    }

    [Fact]
    public void FieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(ForcedProxy._field), 3);

        Proxy._field = 5;

        Assert.Equal(5, Proxy._field);
    }

    [Fact]
    public void ReadonlyFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(ForcedProxy._readonlyField), 3);

        Assert.Equal(3, Proxy._readonlyField);
    }

    [Fact]
    public void ReadonlyFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(ForcedProxy._readonlyField), 3);

        Proxy._readonlyField = 5;

        Assert.Equal(5, Proxy._readonlyField);
    }

    [Fact]
    public void ComplexFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(ForcedProxy._complexField), new OtherType(3));

        Assert.Equal(3, Proxy._complexField.Number);
    }

    [Fact]
    public void ComplexFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(ForcedProxy._complexField), new OtherType(3));

        Proxy._complexField = new OtherType(5);

        Assert.Equal(5, Proxy._complexField.Number);
    }

    [Fact]
    public void PropertyGet()
    {
        ReflectionMemberAccessor.SetPropertyValue(Instance, nameof(ForcedProxy._property), 3);

        Assert.Equal(3, Proxy._property);
    }

    [Fact]
    public void PropertySet()
    {
        ReflectionMemberAccessor.SetPropertyValue(Instance, nameof(ForcedProxy._property), 3);

        Proxy._property = 5;

        Assert.Equal(5, Proxy._property);
    }

    [Fact]
    public void ReadonlyPropertyGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, GetBackingFieldNameFromPropertyName(nameof(ForcedProxy._readonlyProperty)), 3);

        Assert.Equal(3, Proxy._readonlyProperty);
    }

    [Fact]
    public void ReadonlyPropertySet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, GetBackingFieldNameFromPropertyName(nameof(ForcedProxy._readonlyProperty)), 3);

        Proxy._readonlyProperty = 5;

        Assert.Equal(5, Proxy._readonlyProperty);
    }

    [Fact]
    public void StaticFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(instance: null, nameof(TForcedProxy.StaticField), 3);

        Assert.Equal(3, TForcedProxy.StaticField);
    }

    [Fact]
    public void StaticFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(instance: null, nameof(TForcedProxy.StaticField), 3);

        TForcedProxy.StaticField = 5;

        Assert.Equal(5, TForcedProxy.StaticField);
    }

    [Fact]
    public void StaticPropertyGet()
    {
        ReflectionMemberAccessor.SetPropertyValue(instance: null, nameof(TForcedProxy.StaticProperty), 3);

        Assert.Equal(3, TForcedProxy.StaticProperty);
    }

    [Fact]
    public void StaticPropertySet()
    {
        ReflectionMemberAccessor.SetPropertyValue(instance: null, nameof(TForcedProxy.StaticProperty), 3);

        TForcedProxy.StaticProperty = 5;

        Assert.Equal(5, TForcedProxy.StaticProperty);
    }

    [Fact]
    public void StaticProcedureInvocation()
    {
        TForcedProxy.StaticProcedure();

        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("Void StaticProcedure()", method.ToString());
        });
    }

    [Fact]
    public void StaticFunctionInvocation()
    {
        var result = TForcedProxy.StaticFunction();

        Assert.Equal("hello", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String StaticFunction()", method.ToString());
        });
    }

    [Fact]
    public void ProcedureInvocation()
    {
        Proxy.Procedure();

        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("Void Procedure()", method.ToString());
        });
    }

    [Fact]
    public void FunctionInvocation()
    {
        var result = Proxy.Function();

        Assert.Equal("hello", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String Function()", method.ToString());
        });
    }

    [Fact]
    public void ProcedureInvocationWithOneParameter()
    {
        Proxy.Procedure(5);

        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("Void Procedure(Int32)", method.ToString());
        });
    }

    [Fact]
    public void FunctionInvocationWithOneParameter()
    {
        var result = Proxy.Function(5);

        Assert.Equal("5", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String Function(Int32)", method.ToString());
        });
    }

    [Fact]
    public void ProcedureInvocationWithTwoParameter()
    {
        Proxy.Procedure(5, new OtherType(8));

        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("Void Procedure(Int32, NamespaceForOtherTypes.OtherType)", method.ToString());
        });
    }

    [Fact]
    public void FunctionInvocationWithTwoParameter()
    {
        var result = Proxy.Function(5, new OtherType(8));

        Assert.Equal("5, 8", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String Function(Int32, NamespaceForOtherTypes.OtherType)", method.ToString());
        });
    }

    private static string GetBackingFieldNameFromPropertyName(string propertyName) => $"<{propertyName}>k__BackingField";
}
