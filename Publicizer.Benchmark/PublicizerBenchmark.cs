
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Publicizer.Benchmark;

[SimpleJob(runtimeMoniker: RuntimeMoniker.Net48)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
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
    public void StaticProcedureInvocationWith16Parameters()
    {
        OriginalType.StaticProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    public void StaticProcedureInvocationWith17Parameters()
    {
        OriginalType.StaticProcedureWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
    }

    [Benchmark]
    public string StaticFunctionInvocation() => OriginalType.StaticFunction();

    [Benchmark]
    public void StaticFunctionInvocationWith16Parameters() => OriginalType.StaticFunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

    [Benchmark]
    public void StaticFunctionInvocationWith17Parameters() => OriginalType.StaticFunctionWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

    [Benchmark]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    public void ProcedureInvocationWith1Parameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    public void ProcedureInvocationWith2Parameters()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    public void ProcedureInvocationWith15Parameters()
    {
        Instance.ProcedureWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    }

    [Benchmark]
    public void ProcedureInvocationWith16Parameters()
    {
        Instance.ProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    public string FunctionInvocationWith1Parameter() => Instance.Function(5);

    [Benchmark]
    public string FunctionInvocationWith2Parameters() => Instance.Function(5, new OtherType(8));

    [Benchmark]
    public void FunctionInvocationWith15Parameters() => Instance.FunctionWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

    [Benchmark]
    public void FunctionInvocationWith16Parameters() => Instance.FunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
}

[SimpleJob(runtimeMoniker: RuntimeMoniker.Net48)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
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
    public void StaticProcedureInvocationWith16Parameters()
    {
        ForcedProxy.StaticProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    public void StaticProcedureInvocationWith17Parameters()
    {
        ForcedProxy.StaticProcedureWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
    }

    [Benchmark]
    public string StaticFunctionInvocation() => ForcedProxy.StaticFunction();

    [Benchmark]
    public void StaticFunctionInvocationWith16Parameters() => ForcedProxy.StaticFunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

    [Benchmark]
    public void StaticFunctionInvocationWith17Parameters() => ForcedProxy.StaticFunctionWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

    [Benchmark]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    public void ProcedureInvocationWith1Parameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    public void ProcedureInvocationWith2Parameters()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    public void ProcedureInvocationWith15Parameters()
    {
        Instance.ProcedureWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    }

    [Benchmark]
    public void ProcedureInvocationWith16Parameters()
    {
        Instance.ProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    public string FunctionInvocationWith1Parameter() => Instance.Function(5);

    [Benchmark]
    public string FunctionInvocationWith2Parameters() => Instance.Function(5, new OtherType(8));

    [Benchmark]
    public void FunctionInvocationWith15Parameters() => Instance.FunctionWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

    [Benchmark]
    public void FunctionInvocationWith16Parameters() => Instance.FunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
}

[SimpleJob(runtimeMoniker: RuntimeMoniker.Net48)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
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
    public void StaticProcedureInvocationWith16Parameters()
    {
        ForcedProxyWithCustomMemberAccessorType.StaticProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    public void StaticProcedureInvocationWith17Parameters()
    {
        ForcedProxyWithCustomMemberAccessorType.StaticProcedureWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
    }

    [Benchmark]
    public string StaticFunctionInvocation() => ForcedProxyWithCustomMemberAccessorType.StaticFunction();

    [Benchmark]
    public void StaticFunctionInvocationWith16Parameters() => ForcedProxyWithCustomMemberAccessorType.StaticFunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

    [Benchmark]
    public void StaticFunctionInvocationWith17Parameters() => ForcedProxyWithCustomMemberAccessorType.StaticFunctionWith17Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);

    [Benchmark]
    public void ProcedureInvocation()
    {
        Instance.Procedure();
    }

    [Benchmark]
    public void ProcedureInvocationWith1Parameter()
    {
        Instance.Procedure(5);
    }

    [Benchmark]
    public void ProcedureInvocationWith2Parameters()
    {
        Instance.Procedure(5, new OtherType(8));
    }

    [Benchmark]
    public void ProcedureInvocationWith15Parameters()
    {
        Instance.ProcedureWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
    }

    [Benchmark]
    public void ProcedureInvocationWith16Parameters()
    {
        Instance.ProcedureWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
    }

    [Benchmark]
    public string FunctionInvocation() => Instance.Function();

    [Benchmark]
    public string FunctionInvocationWith1Parameter() => Instance.Function(5);

    [Benchmark]
    public string FunctionInvocationWith2Parameters() => Instance.Function(5, new OtherType(8));

    [Benchmark]
    public void FunctionInvocationWith15Parameters() => Instance.FunctionWith15Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

    [Benchmark]
    public void FunctionInvocationWith16Parameters() => Instance.FunctionWith16Parameters(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
}
