# Publicizer

[![NuGet](https://img.shields.io/nuget/v/Publicizer.svg)](https://www.nuget.org/packages/Publicizer) [![NuGet](https://img.shields.io/github/release/davidnemeti/Publicizer?display_name=tag&sort=semver)](../../releases/latest) [![Build](https://github.com/davidnemeti/Publicizer/actions/workflows/build.yml/badge.svg)](https://github.com/davidnemeti/Publicizer/actions/workflows/build.yml) [![License](https://img.shields.io/badge/license-LGPLv3-green)](https://licenses.nuget.org/LGPL-3.0-only)

## Summary

### What does Publicizer do?

Publicizer is a [source generator](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) that let you access the **private members** (*fields*, *properties* and *methods*) of a type from outside with **compile-time safety** in a **typesafe** manner.

### What happens during compile time?

The private members of the type is accessed through a **proxy** type containing generated public members which **forward** to the private members of the original type. Thus, it provides a typesafe way to access the private members. If the name or type of a private member of the original type changes, so does the name or type of the generated public member in the proxy type. Therefore, any *name or type mismatches emerge during compile time*. No more `NullReferenceException`s or `InvalidCastException`s during runtime!

### What happens during runtime?

During **runtime** the actual implementation of forwarding uses **compiled expression trees**, thus providing very **fast** forwarding performance which is almost as fast as accessing directly the original members. Alternatively, any other **custom mechanism** (including a slower, **reflection** based mechanism) can be used to access the private members.

### Extra features

By changing the default `AccessorHandling`, even ***`readonly`** fields* and ***readonly** auto-implemented properties* (having only a **getter**) can be **writable** through the proxy type.

Similarly, by changing the default `AccessorHandling`, ***writeonly** auto-implemented properties* (having only a **setter**) can be **readable** through the proxy.

## Example

### The original type

Let assume the following **type with private members**:

```csharp
public class TypeWithPrivateMembers
{
    private static int StaticField = 3;
    private static int StaticProperty { get; set; } = 8;

    private int _field; = 30;
    private int _property { get; set; } = 80;

    private static void StaticProcedure(int a)
    {
        Console.WriteLine($"{nameof(StaticProcedure)}(int a)");
    }

    private static string StaticFunction(int a)
    {
        Console.WriteLine($"{nameof(StaticFunction)}(int a)");
        return "hello";
    }

    private string Function(int a)
    {
        Console.WriteLine($"{nameof(Function)}(int a)");
        return a.ToString();
    }

    private void Procedure(int a)
    {
        Console.WriteLine($"{nameof(Procedure)}(int a)");
    }
}
```

### The proxy type

In order to access the private members, you need to create a *`partial`* **proxy type** (a **`class`**, **`struct`** or **`record`**, with an arbitrary name), decorate it with the `Publicize` attribute and refer to the original type:

```csharp
[Publicize(typeof(TypeWithPrivateMembers))]
public partial class Proxy
{
}
```

### Usage

#### Accessing instance members

To **access** the **instance** members of the original type through the proxy type, the original type needs to be **instantiated**, as well as the proxy type needs to be instantiated with that instance of the original type:

```csharp
var instance = new TypeWithPrivateMembers();

var proxy = new Proxy(instance);

proxy._field = 38;
proxy._field++;
Console.WriteLine($"_field = {proxy._field}");

proxy._property = 42;
proxy._property++;
Console.WriteLine($"_property = {proxy._property}");

Console.WriteLine(proxy.Function(15));

proxy.Procedure(18);
```

#### Accessing static members

To **access** the **static** members of the original type through the proxy type, the static members of the proxy type can be used:

```csharp
Proxy.StaticField = 38;
Proxy.StaticField++;
Console.WriteLine($"StaticField = {Proxy.StaticField}");

Proxy.StaticProperty = 42;
Proxy.StaticProperty++;
Console.WriteLine($"StaticProperty = {Proxy.StaticProperty}");

Console.WriteLine(Proxy.StaticFunction(15));

Proxy.StaticProcedure(18);
```

### The generated code

Behind the scenes, Publicizer's **source generator** will generate the corresponding public members into the proxy type with the proper *forwarding* code as implementation.
