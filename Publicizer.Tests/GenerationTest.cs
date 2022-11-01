using OuterNamespace.NamespaceForProxyType;

namespace Publicizer.Tests;

public class GenerationTest
{
    [Fact]
    public void ReadonlyField()
    {
        Assert.True(typeof(Proxy).GetProperty(nameof(Proxy._readonlyField))!.CanRead);
        Assert.False(typeof(Proxy).GetProperty(nameof(Proxy._readonlyField))!.CanWrite);
    }

    [Fact]
    public void ReadonlyProperty()
    {
        Assert.True(typeof(Proxy).GetProperty(nameof(Proxy._readonlyProperty))!.CanRead);
        Assert.False(typeof(Proxy).GetProperty(nameof(Proxy._readonlyProperty))!.CanWrite);
    }

    [Fact]
    public void ForcedReadonlyField()
    {
        Assert.True(typeof(ForcedProxy).GetProperty(nameof(ForcedProxy._readonlyField))!.CanRead);
        Assert.True(typeof(ForcedProxy).GetProperty(nameof(ForcedProxy._readonlyField))!.CanWrite);
    }

    [Fact]
    public void ForcedReadonlyProperty()
    {
        Assert.True(typeof(ForcedProxy).GetProperty(nameof(ForcedProxy._readonlyProperty))!.CanRead);
        Assert.True(typeof(ForcedProxy).GetProperty(nameof(ForcedProxy._readonlyProperty))!.CanWrite);
    }

    [Fact]
    public void StaticProxy()
    {
        Assert.All(typeof(StaticProxy).GetMethods(), method => Assert.True(method.IsStatic || method.DeclaringType == typeof(object)));
        Assert.All(typeof(StaticProxy).GetProperties(), property => Assert.True(property.GetGetMethod()!.IsStatic));
    }

    [Fact]
    public void InstanceProxy()
    {
        Assert.All(typeof(InstanceProxy).GetMethods(), method => Assert.False(method.IsStatic));
        Assert.All(typeof(InstanceProxy).GetProperties(), property => Assert.False(property.GetGetMethod()!.IsStatic));
    }
}
