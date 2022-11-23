// NOTE: namespace intentionally differs from folder structure to test the code generation
namespace NamespaceForOtherTypes;

public struct OtherType
{
    public int Number { get; }

    public OtherType(int number)
    {
        Number = number;
    }
}
