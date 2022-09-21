using NamespaceForOtherTypes;
using System.Reflection;

namespace NamespaceForTypeWithPrivateMembers;

public class TypeWithPrivateMembers
{
    private static int StaticField = 3;
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
