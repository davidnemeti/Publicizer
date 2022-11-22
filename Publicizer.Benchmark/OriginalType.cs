namespace Publicizer.Benchmark;

public class OriginalType
{
    public static int StaticField;
    public static int StaticProperty { get; set; }

    public int Field;
    public readonly int ReadonlyField;
    public OtherType ComplexField = new OtherType(0);
    public int ReadonlyProperty { get; }
    public int Property { get; set; }

    public static void StaticProcedure()
    {
        Console.WriteLine("hello");
    }

    public static string StaticFunction()
    {
        return "hello";
    }

    public void Procedure()
    {
        Console.WriteLine("hello");
    }

    public string Function()
    {
        return "hello";
    }

    public void Procedure(int a)
    {
        Console.WriteLine("hello");
    }

    public string Function(int a)
    {
        return a.ToString();
    }

    public void Procedure(int a, OtherType otherType)
    {
        Console.WriteLine("hello");
    }

    public string Function(int a, OtherType otherType)
    {
        return $"{a}, {otherType.Number}";
    }
}
