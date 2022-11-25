
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Publicizer.Benchmark;

public class Benchmark_OriginalType
{
    private readonly OriginalType Instance = new ();

    [Benchmark]
    [BenchmarkCategory("FieldGet")]
    public int FieldGet() => Instance.Field;

    [Benchmark]
    [BenchmarkCategory("FieldSet")]
    public void FieldSet()
    {
        Instance.Field = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ReadonlyFieldGet")]
    public int ReadonlyFieldGet() => Instance.ReadonlyField;

    [Benchmark]
    [BenchmarkCategory("ComplexFieldGet")]
    public OtherType ComplexFieldGet() => Instance.ComplexField;

    [Benchmark]
    [BenchmarkCategory("ComplexFieldSet")]
    public void ComplexFieldSet()
    {
        Instance.ComplexField = new OtherType(5);
    }

    [Benchmark]
    [BenchmarkCategory("PropertyGet")]
    public int PropertyGet() => Instance.Property;

    [Benchmark]
    [BenchmarkCategory("PropertySet")]
    public void PropertySet()
    {
        Instance.Property = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ReadonlyPropertyGet")]
    public int ReadonlyPropertyGet() => Instance.ReadonlyProperty;

    [Benchmark]
    [BenchmarkCategory("StaticFieldGet")]
    public int StaticFieldGet() => OriginalType.StaticField;

    [Benchmark]
    [BenchmarkCategory("StaticFieldSet")]
    public void StaticFieldSet()
    {
        OriginalType.StaticField = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticPropertyGet")]
    public int StaticPropertyGet() => OriginalType.StaticProperty;

    [Benchmark]
    [BenchmarkCategory("StaticPropertySet")]
    public void StaticPropertySet()
    {
        OriginalType.StaticProperty = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocation")]
    public void StaticProcedureInvocation()
    {
        OriginalType.StaticProcedure();
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocationWith16Parameters")]
    public void StaticProcedureInvocationWith16Parameters()
    {
        OriginalType.StaticProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocationWith17Parameters")]
    public void StaticProcedureInvocationWith17Parameters()
    {
        OriginalType.StaticProcedureWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
    }

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocation")]
    public string StaticFunctionInvocation() => OriginalType.StaticFunction();

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocationWith16Parameters")]
    public void StaticFunctionInvocationWith16Parameters() => OriginalType.StaticFunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocationWith17Parameters")]
    public void StaticFunctionInvocationWith17Parameters() => OriginalType.StaticFunctionWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocation")]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith1Parameter")]
    public void ProcedureInvocationWith1Parameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith2Parameters")]
    public void ProcedureInvocationWith2Parameters()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith15Parameters")]
    public void ProcedureInvocationWith15Parameters()
    {
        Instance.ProcedureWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith16Parameters")]
    public void ProcedureInvocationWith16Parameters()
    {
        Instance.ProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    [BenchmarkCategory("FunctionInvocation")]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith1Parameter")]
    public string FunctionInvocationWith1Parameter() => Instance.Function(5);

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith2Parameters")]
    public string FunctionInvocationWith2Parameters() => Instance.Function(5, new OtherType(8));

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith15Parameters")]
    public void FunctionInvocationWith15Parameters() => Instance.FunctionWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith16Parameters")]
    public void FunctionInvocationWith16Parameters() => Instance.FunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
}

public class Benchmark_ExpressionTreeProxy
{
    private readonly ExpressionTreeProxy Instance = new (new OriginalType());

    [Benchmark]
    [BenchmarkCategory("FieldGet")]
    public int FieldGet() => Instance.Field;

    [Benchmark]
    [BenchmarkCategory("FieldSet")]
    public void FieldSet()
    {
        Instance.Field = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ReadonlyFieldGet")]
    public int ReadonlyFieldGet() => Instance.ReadonlyField;

    [Benchmark]
    [BenchmarkCategory("ReadonlyFieldSet")]
    public void ReadonlyFieldSet()
    {
        Instance.ReadonlyField = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ComplexFieldGet")]
    public OtherType ComplexFieldGet() => Instance.ComplexField;

    [Benchmark]
    [BenchmarkCategory("ComplexFieldSet")]
    public void ComplexFieldSet()
    {
        Instance.ComplexField = new OtherType(5);
    }

    [Benchmark]
    [BenchmarkCategory("PropertyGet")]
    public int PropertyGet() => Instance.Property;

    [Benchmark]
    [BenchmarkCategory("PropertySet")]
    public void PropertySet()
    {
        Instance.Property = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ReadonlyPropertyGet")]
    public int ReadonlyPropertyGet() => Instance.ReadonlyProperty;

    [Benchmark]
    [BenchmarkCategory("ReadonlyPropertySet")]
    public void ReadonlyPropertySet()
    {
        Instance.ReadonlyProperty = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticFieldGet")]
    public int StaticFieldGet() => ExpressionTreeProxy.StaticField;

    [Benchmark]
    [BenchmarkCategory("StaticFieldSet")]
    public void StaticFieldSet()
    {
        ExpressionTreeProxy.StaticField = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticPropertyGet")]
    public int StaticPropertyGet() => ExpressionTreeProxy.StaticProperty;

    [Benchmark]
    [BenchmarkCategory("StaticPropertySet")]
    public void StaticPropertySet()
    {
        ExpressionTreeProxy.StaticProperty = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocation")]
    public void StaticProcedureInvocation()
    {
        ExpressionTreeProxy.StaticProcedure();
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocationWith16Parameters")]
    public void StaticProcedureInvocationWith16Parameters()
    {
        ExpressionTreeProxy.StaticProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocationWith17Parameters")]
    public void StaticProcedureInvocationWith17Parameters()
    {
        ExpressionTreeProxy.StaticProcedureWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
    }

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocation")]
    public string StaticFunctionInvocation() => ExpressionTreeProxy.StaticFunction();

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocationWith16Parameters")]
    public void StaticFunctionInvocationWith16Parameters() => ExpressionTreeProxy.StaticFunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocationWith17Parameters")]
    public void StaticFunctionInvocationWith17Parameters() => ExpressionTreeProxy.StaticFunctionWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocation")]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith1Parameter")]
    public void ProcedureInvocationWith1Parameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith2Parameters")]
    public void ProcedureInvocationWith2Parameters()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith15Parameters")]
    public void ProcedureInvocationWith15Parameters()
    {
        Instance.ProcedureWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith16Parameters")]
    public void ProcedureInvocationWith16Parameters()
    {
        Instance.ProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    [BenchmarkCategory("FunctionInvocation")]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith1Parameter")]
    public string FunctionInvocationWith1Parameter() => Instance.Function(5);

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith2Parameters")]
    public string FunctionInvocationWith2Parameters() => Instance.Function(5, new OtherType(8));

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith15Parameters")]
    public void FunctionInvocationWith15Parameters() => Instance.FunctionWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith16Parameters")]
    public void FunctionInvocationWith16Parameters() => Instance.FunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
}

public class Benchmark_ReflectionProxy
{
    private readonly ReflectionProxy Instance = new (new OriginalType());

    [Benchmark]
    [BenchmarkCategory("FieldGet")]
    public int FieldGet() => Instance.Field;

    [Benchmark]
    [BenchmarkCategory("FieldSet")]
    public void FieldSet()
    {
        Instance.Field = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ReadonlyFieldGet")]
    public int ReadonlyFieldGet() => Instance.ReadonlyField;

    [Benchmark]
    [BenchmarkCategory("ReadonlyFieldSet")]
    public void ReadonlyFieldSet()
    {
        Instance.ReadonlyField = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ComplexFieldGet")]
    public OtherType ComplexFieldGet() => Instance.ComplexField;

    [Benchmark]
    [BenchmarkCategory("ComplexFieldSet")]
    public void ComplexFieldSet()
    {
        Instance.ComplexField = new OtherType(5);
    }

    [Benchmark]
    [BenchmarkCategory("PropertyGet")]
    public int PropertyGet() => Instance.Property;

    [Benchmark]
    [BenchmarkCategory("PropertySet")]
    public void PropertySet()
    {
        Instance.Property = 5;
    }

    [Benchmark]
    [BenchmarkCategory("ReadonlyPropertyGet")]
    public int ReadonlyPropertyGet() => Instance.ReadonlyProperty;

    [Benchmark]
    [BenchmarkCategory("ReadonlyPropertySet")]
    public void ReadonlyPropertySet()
    {
        Instance.ReadonlyProperty = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticFieldGet")]
    public int StaticFieldGet() => ReflectionProxy.StaticField;

    [Benchmark]
    [BenchmarkCategory("StaticFieldSet")]
    public void StaticFieldSet()
    {
        ReflectionProxy.StaticField = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticPropertyGet")]
    public int StaticPropertyGet() => ReflectionProxy.StaticProperty;

    [Benchmark]
    [BenchmarkCategory("StaticPropertySet")]
    public void StaticPropertySet()
    {
        ReflectionProxy.StaticProperty = 5;
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocation")]
    public void StaticProcedureInvocation()
    {
        ReflectionProxy.StaticProcedure();
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocationWith16Parameters")]
    public void StaticProcedureInvocationWith16Parameters()
    {
        ReflectionProxy.StaticProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    [BenchmarkCategory("StaticProcedureInvocationWith17Parameters")]
    public void StaticProcedureInvocationWith17Parameters()
    {
        ReflectionProxy.StaticProcedureWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
    }

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocation")]
    public string StaticFunctionInvocation() => ReflectionProxy.StaticFunction();

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocationWith16Parameters")]
    public void StaticFunctionInvocationWith16Parameters() => ReflectionProxy.StaticFunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

    [Benchmark]
    [BenchmarkCategory("StaticFunctionInvocationWith17Parameters")]
    public void StaticFunctionInvocationWith17Parameters() => ReflectionProxy.StaticFunctionWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocation")]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith1Parameter")]
    public void ProcedureInvocationWith1Parameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith2Parameters")]
    public void ProcedureInvocationWith2Parameters()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith15Parameters")]
    public void ProcedureInvocationWith15Parameters()
    {
        Instance.ProcedureWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    }

    [Benchmark]
    [BenchmarkCategory("ProcedureInvocationWith16Parameters")]
    public void ProcedureInvocationWith16Parameters()
    {
        Instance.ProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    [BenchmarkCategory("FunctionInvocation")]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith1Parameter")]
    public string FunctionInvocationWith1Parameter() => Instance.Function(5);

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith2Parameters")]
    public string FunctionInvocationWith2Parameters() => Instance.Function(5, new OtherType(8));

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith15Parameters")]
    public void FunctionInvocationWith15Parameters() => Instance.FunctionWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

    [Benchmark]
    [BenchmarkCategory("FunctionInvocationWith16Parameters")]
    public void FunctionInvocationWith16Parameters() => Instance.FunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
}
