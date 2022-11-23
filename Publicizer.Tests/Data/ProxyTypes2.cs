using Publicizer.Annotation;
using NamespaceForTypeWithPrivateMembers;

// NOTE: embedded namespaces are intentional to test the code generation
namespace Publicizer.Tests.Data
{
    [Publicize(typeof(TypeWithPrivateMembers))]
    internal partial class InternalProxy
    {
    }

    [Publicize(typeof(TypeWithPrivateMembers))]
    public partial struct StructProxy
    {
    }
}
