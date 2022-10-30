using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForProxyType;

var instance = new TypeWithPrivateMembers();

var proxy = new Proxy(instance);

Console.WriteLine($"_field = {proxy._field}");
proxy._field = new OtherType(31);
Console.WriteLine($"_field = {proxy._field}");

Console.WriteLine($"_property = {proxy._property}");
proxy._property++;
Console.WriteLine($"_property = {proxy._property}");

Console.WriteLine($"_readonlyProperty = {proxy._readonlyProperty}");

Console.WriteLine(proxy.Function());
Console.WriteLine(proxy.Function(15));
Console.WriteLine(proxy.Function(15, new OtherType(77)));

proxy.Procedure();
proxy.Procedure(15);
proxy.Procedure(15, new OtherType(77));

Console.WriteLine($"StaticField = {StaticProxy.StaticField}");
StaticProxy.StaticField++;
Console.WriteLine($"StaticField = {StaticProxy.StaticField}");

Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");
StaticProxy.StaticProperty++;
Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");

Console.WriteLine($"StaticReadonlyProperty = {StaticProxy.StaticReadonlyProperty}");

Console.WriteLine(StaticProxy.StaticFunction());

StaticProxy.StaticProcedure();
