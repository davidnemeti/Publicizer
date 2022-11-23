using BenchmarkDotNet.Running;
using Publicizer.Benchmark;
using System.Reflection;

BenchmarkRunner.Run(new[]
{
    typeof(PublicizerBenchmark_OriginalType),
    typeof(PublicizerBenchmark_ForcedProxy),
    typeof(PublicizerBenchmark_ForcedProxyWithCustomMemberAccessorType)
});
