using Publicizer.Annotation;
using Publicizer.Runtime;
using NamespaceForTypeWithPrivateMembers;
using NamespaceForOtherTypes;
using Publicizer.Tests;

// NOTE: embedded namespaces are intentional to test the code generation
namespace OuterNamespace
{
    namespace NamespaceForProxyType
    {
        [Publicize(typeof(TypeWithPrivateMembers))]
        public partial class Proxy
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), AccessorHandling = AccessorHandling.ForceReadAndWrite)]
        public partial class ForcedProxy : IForcedProxy
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), MemberLifetime = MemberLifetime.Instance)]
        public partial class InstanceProxy
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), MemberLifetime = MemberLifetime.Static)]
        public static partial class StaticProxy
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), AccessorHandling = AccessorHandling.ForceReadAndWrite, CustomMemberAccessorType = typeof(ReflectionMemberAccessor<TypeWithPrivateMembers>))]
        public partial class ForcedProxyWithCustomMemberAccessorType : IForcedProxy
        {
        }
    }
}
