using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForProxyType;

namespace Publicizer.Tests;

public class AccessTestForProxy : AccessTest
{
    public AccessTestForProxy()
        : base(instance => new Proxy(instance))
    {
    }
}

public class AccessTestForProxyWithCustomMemberAccessor : AccessTest
{
    public AccessTestForProxyWithCustomMemberAccessor()
        : base(instance => new ProxyWithCustomMemberAccessorType(instance))
    {
    }
}

[UseStaticLogger]
public abstract class AccessTest
{
    protected TypeWithPrivateMembers Instance { get; }
    protected IProxy Proxy { get; }
    protected ReflectionMemberAccessor<TypeWithPrivateMembers> ReflectionMemberAccessor { get; }

    protected AccessTest(Func<TypeWithPrivateMembers, IProxy> createProxy)
    {
        Instance = new();
        Proxy = createProxy(Instance);
        ReflectionMemberAccessor = new ReflectionMemberAccessor<TypeWithPrivateMembers>();
    }

    [Fact]
    public void FieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._field), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, Proxy._field);
    }

    [Fact]
    public void FieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._field), 3, MemberLifetime.All, MemberVisibility.All);

        Proxy._field = 5;

        Assert.Equal(5, Proxy._field);
    }

    [Fact]
    public void ReadonlyFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._readonlyField), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, Proxy._readonlyField);
    }

    [Fact]
    public void ReadonlyFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._readonlyField), 3, MemberLifetime.All, MemberVisibility.All);

        Proxy._readonlyField = 5;

        Assert.Equal(5, Proxy._readonlyField);
    }

    [Fact]
    public void ComplexFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._complexField), new OtherType(3), MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, Proxy._complexField.Number);
    }

    [Fact]
    public void ComplexFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._complexField), new OtherType(3), MemberLifetime.All, MemberVisibility.All);

        Proxy._complexField = new OtherType(5);

        Assert.Equal(5, Proxy._complexField.Number);
    }

    [Fact]
    public void PropertyGet()
    {
        ReflectionMemberAccessor.SetPropertyValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._property), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, Proxy._property);
    }

    [Fact]
    public void PropertySet()
    {
        ReflectionMemberAccessor.SetPropertyValue(Instance, nameof(OuterNamespace.NamespaceForProxyType.Proxy._property), 3, MemberLifetime.All, MemberVisibility.All);

        Proxy._property = 5;

        Assert.Equal(5, Proxy._property);
    }

    [Fact]
    public void ReadonlyPropertyGet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, GetBackingFieldNameFromPropertyName(nameof(OuterNamespace.NamespaceForProxyType.Proxy._readonlyProperty)), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, Proxy._readonlyProperty);
    }

    [Fact(Skip = "Readonly property set is not implemented yet")]
    public void ReadonlyPropertySet()
    {
        ReflectionMemberAccessor.SetFieldValue(Instance, GetBackingFieldNameFromPropertyName(nameof(OuterNamespace.NamespaceForProxyType.Proxy._readonlyProperty)), 3, MemberLifetime.All, MemberVisibility.All);

//        _proxy._readonlyProperty = 5;

        Assert.Equal(5, Proxy._readonlyProperty);
    }

    [Fact]
    public void StaticFieldGet()
    {
        ReflectionMemberAccessor.SetFieldValue(instance: null, nameof(OuterNamespace.NamespaceForProxyType.Proxy.StaticField), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, OuterNamespace.NamespaceForProxyType.Proxy.StaticField);
    }

    [Fact]
    public void StaticFieldSet()
    {
        ReflectionMemberAccessor.SetFieldValue(instance: null, nameof(OuterNamespace.NamespaceForProxyType.Proxy.StaticField), 3, MemberLifetime.All, MemberVisibility.All);

        OuterNamespace.NamespaceForProxyType.Proxy.StaticField = 5;

        Assert.Equal(5, OuterNamespace.NamespaceForProxyType.Proxy.StaticField);
    }

    [Fact]
    public void StaticPropertyGet()
    {
        ReflectionMemberAccessor.SetPropertyValue(instance: null, nameof(OuterNamespace.NamespaceForProxyType.Proxy.StaticProperty), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, OuterNamespace.NamespaceForProxyType.Proxy.StaticProperty);
    }

    [Fact]
    public void StaticPropertySet()
    {
        ReflectionMemberAccessor.SetPropertyValue(instance: null, nameof(OuterNamespace.NamespaceForProxyType.Proxy.StaticProperty), 3, MemberLifetime.All, MemberVisibility.All);

        OuterNamespace.NamespaceForProxyType.Proxy.StaticProperty = 5;

        Assert.Equal(5, OuterNamespace.NamespaceForProxyType.Proxy.StaticProperty);
    }

    [Fact]
    public void StaticProcedureInvocation()
    {
        OuterNamespace.NamespaceForProxyType.Proxy.StaticProcedure();

        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("Void StaticProcedure()", method.ToString());
        });
    }

    [Fact]
    public void StaticFunctionInvocation()
    {
        var result = OuterNamespace.NamespaceForProxyType.Proxy.StaticFunction();

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
