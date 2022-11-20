using Publicizer;
using NamespaceForTypeWithPrivateMembers;
using NamespaceForOtherTypes;

// NOTE: embedded namespaces are intentional to test the code generation
namespace OuterNamespace
{
    namespace NamespaceForProxyType
    {
        public interface IForcedProxy
        {
            OtherType _complexField { get; set; }
            int _field { get; set; }
            int _property { get; set; }
            int _readonlyField { get; set; }
            int _readonlyProperty { get; set; }

            string Function();
            string Function(int a);
            string Function(int a, OtherType otherType);
            void Procedure();
            void Procedure(int a);
            void Procedure(int a, OtherType otherType);

            static abstract int StaticField { get; set; }
            static abstract int StaticProperty { get; set; }

            static abstract void StaticProcedure();
            static abstract string StaticFunction();
        }

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
