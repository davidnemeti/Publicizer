// See https://aka.ms/new-console-template for more information

using NamespaceForOtherTypes;
using NamespaceForTypeWithPrivateMembers;
using OuterNamespace.NamespaceForForwarderType;

Console.WriteLine("Hello, World!");

var instance = new TypeWithPrivateMembers();

var forwarder = new ForwarderType(instance);

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

Console.WriteLine($"StaticField = {StaticForwarderType.StaticField}");
StaticForwarderType.StaticField++;
Console.WriteLine($"StaticField = {StaticForwarderType.StaticField}");

Console.WriteLine($"StaticProperty = {StaticForwarderType.StaticProperty}");
StaticForwarderType.StaticProperty++;
Console.WriteLine($"StaticProperty = {StaticForwarderType.StaticProperty}");

Console.WriteLine($"StaticReadonlyProperty = {StaticForwarderType.StaticReadonlyProperty}");

Console.WriteLine(StaticForwarderType.StaticFunction());

StaticForwarderType.StaticProcedure();
