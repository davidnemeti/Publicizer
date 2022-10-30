using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForProxyType;

namespace Publicizer.Tests;

public class AccessTest : IDisposable
{
    private readonly TypeWithPrivateMembers _instance;
    private readonly Proxy _proxy;
    private readonly ReflectionMemberAccessor<TypeWithPrivateMembers> _reflectionMemberAccessor;

    public AccessTest()
    {
        _instance = new();
        _proxy = new(_instance);
        _reflectionMemberAccessor = new ReflectionMemberAccessor<TypeWithPrivateMembers>();
    }

    [Fact]
    public void FieldGet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, nameof(Proxy._field), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, _proxy._field);
    }

    [Fact]
    public void FieldSet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, nameof(Proxy._field), 3, MemberLifetime.All, MemberVisibility.All);

        _proxy._field = 5;

        Assert.Equal(5, _proxy._field);
    }

    [Fact]
    public void ReadonlyFieldGet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, nameof(Proxy._readonlyField), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, _proxy._readonlyField);
    }

    [Fact]
    public void ReadonlyFieldSet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, nameof(Proxy._readonlyField), 3, MemberLifetime.All, MemberVisibility.All);

        _proxy._readonlyField = 5;

        Assert.Equal(5, _proxy._readonlyField);
    }

    [Fact]
    public void ComplexFieldGet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, nameof(Proxy._complexField), new OtherType(3), MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, _proxy._complexField.Number);
    }

    [Fact]
    public void ComplexFieldSet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, nameof(Proxy._complexField), new OtherType(3), MemberLifetime.All, MemberVisibility.All);

        _proxy._complexField = new OtherType(5);

        Assert.Equal(5, _proxy._complexField.Number);
    }

    [Fact]
    public void PropertyGet()
    {
        _reflectionMemberAccessor.SetPropertyValue(_instance, nameof(Proxy._property), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, _proxy._property);
    }

    [Fact]
    public void PropertySet()
    {
        _reflectionMemberAccessor.SetPropertyValue(_instance, nameof(Proxy._property), 3, MemberLifetime.All, MemberVisibility.All);

        _proxy._property = 5;

        Assert.Equal(5, _proxy._property);
    }

    [Fact]
    public void ReadonlyPropertyGet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, GetBackingFieldNameFromPropertyName(nameof(Proxy._readonlyProperty)), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, _proxy._readonlyProperty);
    }

    [Fact(Skip = "Readonly property set is not implemented yet")]
    public void ReadonlyPropertySet()
    {
        _reflectionMemberAccessor.SetFieldValue(_instance, GetBackingFieldNameFromPropertyName(nameof(Proxy._readonlyProperty)), 3, MemberLifetime.All, MemberVisibility.All);

//        _proxy._readonlyProperty = 5;

        Assert.Equal(5, _proxy._readonlyProperty);
    }

    [Fact]
    public void StaticFieldGet()
    {
        _reflectionMemberAccessor.SetFieldValue(instance: null, nameof(Proxy.StaticField), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, Proxy.StaticField);
    }

    [Fact]
    public void StaticFieldSet()
    {
        _reflectionMemberAccessor.SetFieldValue(instance: null, nameof(Proxy.StaticField), 3, MemberLifetime.All, MemberVisibility.All);

        Proxy.StaticField = 5;

        Assert.Equal(5, Proxy.StaticField);
    }

    [Fact]
    public void StaticPropertyGet()
    {
        _reflectionMemberAccessor.SetPropertyValue(instance: null, nameof(Proxy.StaticProperty), 3, MemberLifetime.All, MemberVisibility.All);

        Assert.Equal(3, Proxy.StaticProperty);
    }

    [Fact]
    public void StaticPropertySet()
    {
        _reflectionMemberAccessor.SetPropertyValue(instance: null, nameof(Proxy.StaticProperty), 3, MemberLifetime.All, MemberVisibility.All);

        Proxy.StaticProperty = 5;

        Assert.Equal(5, Proxy.StaticProperty);
    }

    [Fact]
    public void StaticProcedureInvocation()
    {
        Proxy.StaticProcedure();

        Assert.Collection(StaticLogger.LoggedMethods, method =>
            {
                Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
                Assert.Equal("Void StaticProcedure()", method.ToString());
            }
        );
    }

    [Fact]
    public void StaticFunctionInvocation()
    {
        var result = Proxy.StaticFunction();

        Assert.Equal("hello", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String StaticFunction()", method.ToString());
        }
        );
    }

    [Fact]
    public void ProcedureInvocation()
    {
        _proxy.Procedure();

        Assert.Collection(StaticLogger.LoggedMethods, method =>
            {
                Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
                Assert.Equal("Void Procedure()", method.ToString());
            }
        );
    }

    [Fact]
    public void FunctionInvocation()
    {
        var result = _proxy.Function();

        Assert.Equal("hello", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String Function()", method.ToString());
        }
        );
    }

    [Fact]
    public void ProcedureInvocationWithOneParameter()
    {
        _proxy.Procedure(5);

        Assert.Collection(StaticLogger.LoggedMethods, method =>
            {
                Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
                Assert.Equal("Void Procedure(Int32)", method.ToString());
            }
        );
    }

    [Fact]
    public void FunctionInvocationWithOneParameter()
    {
        var result = _proxy.Function(5);

        Assert.Equal("5", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String Function(Int32)", method.ToString());
        }
        );
    }

    [Fact]
    public void ProcedureInvocationWithTwoParameter()
    {
        _proxy.Procedure(5, new OtherType(8));

        Assert.Collection(StaticLogger.LoggedMethods, method =>
            {
                Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
                Assert.Equal("Void Procedure(Int32, NamespaceForOtherTypes.OtherType)", method.ToString());
            }
        );
    }

    [Fact]
    public void FunctionInvocationWithTwoParameter()
    {
        var result = _proxy.Function(5, new OtherType(8));

        Assert.Equal("5, 8", result);
        Assert.Collection(StaticLogger.LoggedMethods, method =>
        {
            Assert.Equal(typeof(TypeWithPrivateMembers), method.DeclaringType);
            Assert.Equal("System.String Function(Int32, NamespaceForOtherTypes.OtherType)", method.ToString());
        }
        );
    }

    private static string GetBackingFieldNameFromPropertyName(string propertyName) => $"<{propertyName}>k__BackingField";

    public void Dispose()
    {
        StaticLogger.Reset();
    }
}
