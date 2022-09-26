// See https://aka.ms/new-console-template for more information

using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForForwarderType;

Console.WriteLine("Hello, World!");

var instance = new TypeWithPrivateMembers();

var forwarder = new Proxy(instance);

Console.WriteLine($"_field = {forwarder._field}");
forwarder._field = new OtherType(31);
Console.WriteLine($"_field = {forwarder._field}");

Console.WriteLine($"_property = {forwarder._property}");
forwarder._property++;
Console.WriteLine($"_property = {forwarder._property}");

Console.WriteLine($"_readonlyProperty = {forwarder._readonlyProperty}");

Console.WriteLine(forwarder.Function());
Console.WriteLine(forwarder.Function(15));
Console.WriteLine(forwarder.Function(15, new OtherType(77)));

forwarder.Procedure();
forwarder.Procedure(15);
forwarder.Procedure(15, new OtherType(77));

Console.WriteLine($"StaticField = {StaticProxy.StaticField}");
StaticProxy.StaticField++;
Console.WriteLine($"StaticField = {StaticProxy.StaticField}");

Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");
StaticProxy.StaticProperty++;
Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");

Console.WriteLine($"StaticReadonlyProperty = {StaticProxy.StaticReadonlyProperty}");

Console.WriteLine(StaticProxy.StaticFunction());

StaticProxy.StaticProcedure();
