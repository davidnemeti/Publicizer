
using BenchmarkDotNet.Attributes;

namespace Publicizer.Benchmark;

public class PublicizerBenchmark_OriginalType
{
    private readonly OriginalType Instance = new ();

    [Benchmark]
    public int FieldGet() => Instance.Field;

    [Benchmark]
    public void FieldSet()
    {
        Instance.Field = 5;
    }

    [Benchmark]
    public int ReadonlyFieldGet() => Instance.ReadonlyField;

    [Benchmark]
    public OtherType ComplexFieldGet() => Instance.ComplexField;

    [Benchmark]
    public void ComplexFieldSet()
    {
        Instance.ComplexField = new OtherType(5);
    }

    [Benchmark]
    public int PropertyGet() => Instance.Property;

    [Benchmark]
    public void PropertySet()
    {
        Instance.Property = 5;
    }

    [Benchmark]
    public int ReadonlyPropertyGet() => Instance.ReadonlyProperty;

    [Benchmark]
    public int StaticFieldGet() => OriginalType.StaticField;

    [Benchmark]
    public void StaticFieldSet()
    {
        OriginalType.StaticField = 5;
    }

    [Benchmark]
    public int StaticPropertyGet() => OriginalType.StaticProperty;

    [Benchmark]
    public void StaticPropertySet()
    {
        OriginalType.StaticProperty = 5;
    }

    [Benchmark]
    public void StaticProcedureInvocation()
    {
        OriginalType.StaticProcedure();
    }

    [Benchmark]
    public string StaticFunctionInvocation() => OriginalType.StaticFunction();

    [Benchmark]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    public void ProcedureInvocationWithOneParameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    public string FunctionInvocationWithOneParameter() => Instance.Function(5);

    [Benchmark]
    public void ProcedureInvocationWithTwoParameter()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    public string FunctionInvocationWithTwoParameter() => Instance.Function(5, new OtherType(8));
}

public class PublicizerBenchmark_ForcedProxy
{
    private readonly ForcedProxy Instance = new (new OriginalType());

    [Benchmark]
    public int FieldGet() => Instance.Field;

    [Benchmark]
    public void FieldSet()
    {
        Instance.Field = 5;
    }

    [Benchmark]
    public int ReadonlyFieldGet() => Instance.ReadonlyField;

    [Benchmark]
    public void ReadonlyFieldSet()
    {
        Instance.ReadonlyField = 5;
    }

    [Benchmark]
    public OtherType ComplexFieldGet() => Instance.ComplexField;

    [Benchmark]
    public void ComplexFieldSet()
    {
        Instance.ComplexField = new OtherType(5);
    }

    [Benchmark]
    public int PropertyGet() => Instance.Property;

    [Benchmark]
    public void PropertySet()
    {
        Instance.Property = 5;
    }

    [Benchmark]
    public int ReadonlyPropertyGet() => Instance.ReadonlyProperty;

    [Benchmark]
    public void ReadonlyPropertySet()
    {
        Instance.ReadonlyProperty = 5;
    }

    [Benchmark]
    public int StaticFieldGet() => ForcedProxy.StaticField;

    [Benchmark]
    public void StaticFieldSet()
    {
        ForcedProxy.StaticField = 5;
    }

    [Benchmark]
    public int StaticPropertyGet() => ForcedProxy.StaticProperty;

    [Benchmark]
    public void StaticPropertySet()
    {
        ForcedProxy.StaticProperty = 5;
    }

    [Benchmark]
    public void StaticProcedureInvocation()
    {
        ForcedProxy.StaticProcedure();
    }

    [Benchmark]
    public string StaticFunctionInvocation() => ForcedProxy.StaticFunction();

    [Benchmark]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    public void ProcedureInvocationWithOneParameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    public string FunctionInvocationWithOneParameter() => Instance.Function(5);

    [Benchmark]
    public void ProcedureInvocationWithTwoParameter()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    public string FunctionInvocationWithTwoParameter() => Instance.Function(5, new OtherType(8));
}

public class PublicizerBenchmark_ForcedProxyWithCustomMemberAccessorType
{
    private readonly ForcedProxyWithCustomMemberAccessorType Instance = new (new OriginalType());

    [Benchmark]
    public int FieldGet() => Instance.Field;

    [Benchmark]
    public void FieldSet()
    {
        Instance.Field = 5;
    }

    [Benchmark]
    public int ReadonlyFieldGet() => Instance.ReadonlyField;

    [Benchmark]
    public void ReadonlyFieldSet()
    {
        Instance.ReadonlyField = 5;
    }

    [Benchmark]
    public OtherType ComplexFieldGet() => Instance.ComplexField;

    [Benchmark]
    public void ComplexFieldSet()
    {
        Instance.ComplexField = new OtherType(5);
    }

    [Benchmark]
    public int PropertyGet() => Instance.Property;

    [Benchmark]
    public void PropertySet()
    {
        Instance.Property = 5;
    }

    [Benchmark]
    public int ReadonlyPropertyGet() => Instance.ReadonlyProperty;

    [Benchmark]
    public void ReadonlyPropertySet()
    {
        Instance.ReadonlyProperty = 5;
    }

    [Benchmark]
    public int StaticFieldGet() => ForcedProxyWithCustomMemberAccessorType.StaticField;

    [Benchmark]
    public void StaticFieldSet()
    {
        ForcedProxyWithCustomMemberAccessorType.StaticField = 5;
    }

    [Benchmark]
    public int StaticPropertyGet() => ForcedProxyWithCustomMemberAccessorType.StaticProperty;

    [Benchmark]
    public void StaticPropertySet()
    {
        ForcedProxyWithCustomMemberAccessorType.StaticProperty = 5;
    }

    [Benchmark]
    public void StaticProcedureInvocation()
    {
        ForcedProxyWithCustomMemberAccessorType.StaticProcedure();
    }

    [Benchmark]
    public string StaticFunctionInvocation() => ForcedProxyWithCustomMemberAccessorType.StaticFunction();

    [Benchmark]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    public void ProcedureInvocationWithOneParameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    public string FunctionInvocationWithOneParameter() => Instance.Function(5);

    [Benchmark]
    public void ProcedureInvocationWithTwoParameter()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    public string FunctionInvocationWithTwoParameter() => Instance.Function(5, new OtherType(8));
}
