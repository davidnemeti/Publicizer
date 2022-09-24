using Publicizer;
using NamespaceForTypeWithPrivateMembers;

namespace OuterNamespace
{
    namespace NamespaceForForwarderType
    {
        [Publicize(typeof(TypeWithPrivateMembers), MemberLifetime.Instance)]
        public partial class ProxyForTypeWithPrivateMembers
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), MemberLifetime.Static)]
        public static partial class StaticProxyForTypeWithPrivateMembers
        {
        }
    }
}
