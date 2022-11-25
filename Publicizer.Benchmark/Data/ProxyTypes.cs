using Publicizer.Annotation;
using Publicizer.Runtime;

namespace Publicizer.Benchmark;

[Publicize(typeof(OriginalType), AccessorHandling = AccessorHandling.ForceReadAndWrite)]
public partial class ExpressionTreeProxy
{
}

[Publicize(typeof(OriginalType), AccessorHandling = AccessorHandling.ForceReadAndWrite, CustomMemberAccessorType = typeof(ReflectionMemberAccessor))]
public partial class ReflectionProxy
{
}
