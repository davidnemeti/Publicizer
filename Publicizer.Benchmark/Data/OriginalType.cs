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
        StaticField = 3;    // at least do something
    }

    public static void StaticProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        StaticField = a3;    // at least do something
    }

    public static void StaticProcedureWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17)
    {
        StaticField = a3;    // at least do something
    }

    public static string StaticFunction()
    {
        return "hello";
    }

    public static string StaticFunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        return "hello";
    }

    public static string StaticFunctionWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17)
    {
        return "hello";
    }

    public void Procedure()
    {
        Field = 3;      // at least do something
    }

    public string Function()
    {
        return "hello";
    }

    public void Procedure(int a)
    {
        Field = a;      // at least do something
    }

    public string Function(int a)
    {
        return a.ToString();
    }

    public void Procedure(int a, OtherType otherType)
    {
        Field = a;      // at least do something
    }

    public string Function(int a, OtherType otherType)
    {
        return $"{a}, {otherType.Number}";
    }

    public void ProcedureWith15Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15)
    {
        Field = a3;      // at least do something
    }

    public void ProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        Field = a3;      // at least do something
    }

    public string FunctionWith15Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15)
    {
        return "hello";
    }

    public string FunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        return "hello";
    }
}
