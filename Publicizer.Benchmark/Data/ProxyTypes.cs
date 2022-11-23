using Publicizer.Annotation;
using Publicizer.Runtime;

namespace Publicizer.Benchmark;

[Publicize(typeof(OriginalType), AccessorHandling = AccessorHandling.ForceReadAndWrite)]
public partial class ForcedProxy
{
}

[Publicize(typeof(OriginalType), AccessorHandling = AccessorHandling.ForceReadAndWrite, CustomMemberAccessorType = typeof(ReflectionMemberAccessor))]
public partial class ForcedProxyWithCustomMemberAccessorType
{
}
