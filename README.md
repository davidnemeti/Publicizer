# Publicizer

## Summary

Publicizer is a [source generator](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) that let you access the **private members** of a class from outside in a **typesafe** manner.

The private members of the class is accessed through a **proxy** class which contains the generated public members which **forward** to the private members of the original class. Thus, it provides a typesafe way to access the private members. If the name or type of a private member of the original class changes, so does the name or type of the generated public member in the proxy class. Therefore, any **name or type mismatches emerge during compile time**. No more `NullReferenceException`s or `InvalidCastException`s!

During **runtime** the actual implementation of forwarding uses **reflection** (or any other **custom mechanism**) to access the private members.

## NuGet

Publicizer can be used as a nuget package:

[![#](https://img.shields.io/nuget/v/Publicizer.svg)](https://www.nuget.org/packages/Publicizer)

## Example

### The original class

Let assume the following **class with private members**:

```csharp
public class TypeWithPrivateMembers
{
#pragma warning disable CS0414 // The field 'TypeWithPrivateMembers.StaticField' is assigned but its value is never used
    private static int StaticField = 3;
#pragma warning restore CS0414 // The field 'TypeWithPrivateMembers.StaticField' is assigned but its value is never used
    private static int StaticReadonlyProperty { get; } = 5;
    private static int StaticProperty { get; set; } = 8;

    private OtherType _field = new OtherType(30);
    private int _readonlyProperty { get; } = 50;
    private int _property { get; set; } = 80;

    private static void StaticProcedure()
    {
        Console.WriteLine($"{nameof(StaticProcedure)}()");
    }

    private static string StaticFunction()
    {
        Console.WriteLine($"{nameof(StaticFunction)}()");
        return "hello";
    }

    private void Procedure()
    {
        Console.WriteLine($"{nameof(Procedure)}()");
    }

    private string Function()
    {
        Console.WriteLine($"{nameof(Function)}()");
        return "hello";
    }

    private void Procedure(int a)
    {
        Console.WriteLine($"{nameof(Procedure)}(int a)");
    }

    private string Function(int a)
    {
        Console.WriteLine($"{nameof(Function)}(int a)");
        return a.ToString();
    }

    private void Procedure(int a, OtherType otherType)
    {
        Console.WriteLine($"{nameof(Procedure)}(int a, OtherType otherType)");
    }

    private string Function(int a, OtherType otherType)
    {
        Console.WriteLine($"{nameof(Function)}(int a, OtherType otherType)");
        return a.ToString();
    }
}
```

### The proxy class

In order to access the private members, you need to create a *partial* **proxy class** (with an arbitrary name), decorate it with the `Publicize` attribute and refer to the original class:

```csharp
[Publicize(typeof(TypeWithPrivateMembers))]
public partial class Proxy
{
}
```

### The generated code

Publicizer's **source generator will generate** the corresponding public members into the proxy class with the proper *forwarding* code as implementation:

```csharp
public partial class Proxy
{
    private static readonly global::Publicizer.IMemberAccessor<global::NamespaceForTypeWithPrivateMembers.TypeWithPrivateMembers> Proxy_IMemberAccessor = new global::Publicizer.ReflectionMemberAccessor<global::NamespaceForTypeWithPrivateMembers.TypeWithPrivateMembers>();
    
    private readonly global::NamespaceForTypeWithPrivateMembers.TypeWithPrivateMembers Proxy_TypeWithPrivateMembers;
    
    public Proxy(global::NamespaceForTypeWithPrivateMembers.TypeWithPrivateMembers instanceToPublicize)
    {
        this.Proxy_TypeWithPrivateMembers = instanceToPublicize;
    }
    
    public static int StaticField
    {
        get => Proxy_IMemberAccessor.GetFieldValue<int>(null, "StaticField", global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
        set => Proxy_IMemberAccessor.SetFieldValue<int>(null, "StaticField", value, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    }
    
    public static int StaticReadonlyProperty
    {
        get => Proxy_IMemberAccessor.GetPropertyValue<int>(null, "StaticReadonlyProperty", global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    }
    
    public static int StaticProperty
    {
        get => Proxy_IMemberAccessor.GetPropertyValue<int>(null, "StaticProperty", global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
        set => Proxy_IMemberAccessor.SetPropertyValue<int>(null, "StaticProperty", value, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    }
    
    public global::NamespaceForOtherTypes.OtherType _field
    {
        get => Proxy_IMemberAccessor.GetFieldValue<global::NamespaceForOtherTypes.OtherType>(this.Proxy_TypeWithPrivateMembers, "_field", global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
        set => Proxy_IMemberAccessor.SetFieldValue<global::NamespaceForOtherTypes.OtherType>(this.Proxy_TypeWithPrivateMembers, "_field", value, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    }
    
    public int _readonlyProperty
    {
        get => Proxy_IMemberAccessor.GetPropertyValue<int>(this.Proxy_TypeWithPrivateMembers, "_readonlyProperty", global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    }
    
    public int _property
    {
        get => Proxy_IMemberAccessor.GetPropertyValue<int>(this.Proxy_TypeWithPrivateMembers, "_property", global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
        set => Proxy_IMemberAccessor.SetPropertyValue<int>(this.Proxy_TypeWithPrivateMembers, "_property", value, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    }
    
    public static void StaticProcedure() =>
        Proxy_IMemberAccessor.InvokeMethod(null, "StaticProcedure", new Type[] {  }, new object[] {  }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    
    public static string StaticFunction() =>
        (string) Proxy_IMemberAccessor.InvokeMethod(null, "StaticFunction", new Type[] {  }, new object[] {  }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    
    public void Procedure() =>
        Proxy_IMemberAccessor.InvokeMethod(this.Proxy_TypeWithPrivateMembers, "Procedure", new Type[] {  }, new object[] {  }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    
    public string Function() =>
        (string) Proxy_IMemberAccessor.InvokeMethod(this.Proxy_TypeWithPrivateMembers, "Function", new Type[] {  }, new object[] {  }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    
    public void Procedure(int a) =>
        Proxy_IMemberAccessor.InvokeMethod(this.Proxy_TypeWithPrivateMembers, "Procedure", new Type[] { typeof(int) }, new object[] { a }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    
    public string Function(int a) =>
        (string) Proxy_IMemberAccessor.InvokeMethod(this.Proxy_TypeWithPrivateMembers, "Function", new Type[] { typeof(int) }, new object[] { a }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    
    public void Procedure(int a, global::NamespaceForOtherTypes.OtherType otherType) =>
        Proxy_IMemberAccessor.InvokeMethod(this.Proxy_TypeWithPrivateMembers, "Procedure", new Type[] { typeof(int), typeof(global::NamespaceForOtherTypes.OtherType) }, new object[] { a, otherType }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
    
    public string Function(int a, global::NamespaceForOtherTypes.OtherType otherType) =>
        (string) Proxy_IMemberAccessor.InvokeMethod(this.Proxy_TypeWithPrivateMembers, "Function", new Type[] { typeof(int), typeof(global::NamespaceForOtherTypes.OtherType) }, new object[] { a, otherType }, global::Publicizer.MemberLifetime.All, global::Publicizer.MemberVisibility.All);
}
```

### Usage

#### Accessing instance members

To **access** the **instance** members of the original class through the proxy class, the original class needs to be **instantiated**, as well as the proxy class needs to be instantiated with that instance of the original class:

```csharp
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
```

#### Accessing static members

To **access** the **static** members of the original class through the proxy class, the static members of the proxy class can be used:

```csharp
Console.WriteLine($"StaticField = {StaticProxy.StaticField}");
StaticProxy.StaticField++;
Console.WriteLine($"StaticField = {StaticProxy.StaticField}");

Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");
StaticProxy.StaticProperty++;
Console.WriteLine($"StaticProperty = {StaticProxy.StaticProperty}");

Console.WriteLine($"StaticReadonlyProperty = {StaticProxy.StaticReadonlyProperty}");

Console.WriteLine(StaticProxy.StaticFunction());

StaticProxy.StaticProcedure();
```
