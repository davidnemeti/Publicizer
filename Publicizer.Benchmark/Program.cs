using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Publicizer.Benchmark;

var benchmarkTypes = new[]
{
    typeof(Benchmark_OriginalType),
    typeof(Benchmark_ExpressionTreeProxy),
    typeof(Benchmark_ReflectionProxy)
};

var job = Job.Default;

BenchmarkRunner.Run(benchmarkTypes, CreateConfig().AddJob(job.WithRuntime(CoreRuntime.Core70)));
BenchmarkRunner.Run(benchmarkTypes, CreateConfig().AddJob(job.WithRuntime(ClrRuntime.Net48)));

IConfig CreateConfig() =>
    ManualConfig
        .Create(DefaultConfig.Instance)
        .WithOptions(ConfigOptions.JoinSummary)
        .AddDiagnoser(MemoryDiagnoser.Default)
        .AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory);
