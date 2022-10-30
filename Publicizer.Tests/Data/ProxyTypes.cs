using Publicizer;
using NamespaceForTypeWithPrivateMembers;
using NamespaceForOtherTypes;

// NOTE: embedded namespaces are intentional to test the code generation
namespace OuterNamespace
{
    namespace NamespaceForProxyType
    {
        public interface IProxy
        {
            OtherType _complexField { get; set; }
            int _field { get; set; }
            int _property { get; set; }
            int _readonlyField { get; set; }
            int _readonlyProperty { get; }

            string Function();
            string Function(int a);
            string Function(int a, OtherType otherType);
            void Procedure();
            void Procedure(int a);
            void Procedure(int a, OtherType otherType);
        }

        [Publicize(typeof(TypeWithPrivateMembers))]
        public partial class Proxy : IProxy
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

        [Publicize(typeof(TypeWithPrivateMembers), customMemberAccessorType: typeof(CustomMemberAccessor<TypeWithPrivateMembers>))]
        public partial class ProxyWithCustomMemberAccessorType : IProxy
        {
        }
    }
}
