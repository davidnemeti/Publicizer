using NamespaceForOtherTypes;
using Publicizer.Tests;

namespace NamespaceForTypeWithPrivateMembers;

public class TypeWithPrivateMembers
{
#pragma warning disable CS0169 // The field 'TypeWithPrivateMembers._readonlyField' is never used
    private static int StaticField;
#pragma warning restore CS0169 // The field 'TypeWithPrivateMembers._readonlyField' is never used
    private static int StaticProperty { get; set; }

#pragma warning disable CS0169 // The field 'TypeWithPrivateMembers._readonlyField' is never used
    private int _field;
    private readonly int _readonlyField;
#pragma warning restore CS0169 // The field 'TypeWithPrivateMembers._readonlyField' is never used
    private OtherType _complexField = new OtherType(0);
    private int _readonlyProperty { get; }
    private int _property { get; set; }

    private static void StaticProcedure()
    {
        StaticLogger.Log();
    }

    private static string StaticFunction()
    {
        StaticLogger.Log();
        return "hello";
    }

    private void Procedure()
    {
        StaticLogger.Log();
    }

    private string Function()
    {
        StaticLogger.Log();
        return "hello";
    }

    private void Procedure(int a)
    {
        StaticLogger.Log();
    }

    private string Function(int a)
    {
        StaticLogger.Log();
        return a.ToString();
    }

    private void Procedure(int a, OtherType otherType)
    {
        StaticLogger.Log();
    }

    private string Function(int a, OtherType otherType)
    {
        StaticLogger.Log();
        return $"{a}, {otherType.Number}";
    }
}
