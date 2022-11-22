using Publicizer.Annotation;
using Publicizer.Runtime;
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
        }

        public interface IForcedProxyStatic
        {
            int StaticField { get; set; }
            int StaticProperty { get; set; }

            void StaticProcedure();
            string StaticFunction();
        }

        public class ForcedProxyStatic : IForcedProxyStatic
        {
            public int StaticField { get => ForcedProxy.StaticField; set => ForcedProxy.StaticField = value; }
            public int StaticProperty { get => ForcedProxy.StaticProperty; set => ForcedProxy.StaticProperty = value; }

            public string StaticFunction() => ForcedProxy.StaticFunction();
            public void StaticProcedure() => ForcedProxy.StaticProcedure();
        }

        public class ForcedProxyWithCustomMemberAccessorTypeStatic : IForcedProxyStatic
        {
            public int StaticField { get => ForcedProxyWithCustomMemberAccessorType.StaticField; set => ForcedProxyWithCustomMemberAccessorType.StaticField = value; }
            public int StaticProperty { get => ForcedProxyWithCustomMemberAccessorType.StaticProperty; set => ForcedProxyWithCustomMemberAccessorType.StaticProperty = value; }

            public string StaticFunction() => ForcedProxyWithCustomMemberAccessorType.StaticFunction();
            public void StaticProcedure() => ForcedProxyWithCustomMemberAccessorType.StaticProcedure();
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
