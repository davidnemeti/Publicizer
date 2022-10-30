using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForProxyType;
using System.Diagnostics.CodeAnalysis;

namespace Publicizer.Tests;

[TestClass]
public class PublicizerTest
{
    private TypeWithPrivateMembers _instance = null!;
    private Proxy _proxy = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _instance = new();
        _proxy = new(_instance);
    }

    [TestMethod]
    public void TestMethod1()
    {
        Console.WriteLine($"_field = {_proxy._field}");
        _proxy._field = new OtherType(31);
        Console.WriteLine($"_field = {_proxy._field}");

        Console.WriteLine($"_property = {_proxy._property}");
        _proxy._property++;
        Console.WriteLine($"_property = {_proxy._property}");

        Console.WriteLine($"_readonlyProperty = {_proxy._readonlyProperty}");

        Console.WriteLine(_proxy.Function());
        Console.WriteLine(_proxy.Function(15));
        Console.WriteLine(_proxy.Function(15, new OtherType(77)));

        _proxy.Procedure();
        _proxy.Procedure(15);
        _proxy.Procedure(15, new OtherType(77));

        Console.WriteLine($"StaticField = {StaticProxy.StaticField}");
        StaticProxy.StaticField++;
        Console.WriteLine($"StaticField = {StaticProxy.StaticField}");

        Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");
        StaticProxy.StaticProperty++;
        Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");

        Console.WriteLine($"StaticReadonlyProperty = {StaticProxy.StaticReadonlyProperty}");

        Console.WriteLine(StaticProxy.StaticFunction());

        StaticProxy.StaticProcedure();
    }
}
