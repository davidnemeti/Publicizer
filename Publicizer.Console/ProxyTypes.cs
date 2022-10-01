using Publicizer;
using NamespaceForTypeWithPrivateMembers;

namespace OuterNamespace
{
    namespace NamespaceForForwarderType
    {
        [Publicize(typeof(TypeWithPrivateMembers))]
        public partial class Proxy
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), MemberLifetime.Instance)]
        public partial class InstanceProxy
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), MemberLifetime.Static)]
        public static partial class StaticProxy
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), customMemberAccessorType: typeof(SpecialMemberAccessor<TypeWithPrivateMembers>))]
        public partial class ProxyWithSpecialMemberAccessorType
        {
        }
    }
}
