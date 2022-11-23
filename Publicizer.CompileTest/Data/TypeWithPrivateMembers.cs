using NamespaceForOtherTypes;

namespace NamespaceForTypeWithPrivateMembers
{
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
        }

        private static void StaticProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
        {
        }

        private static void StaticProcedureWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17)
        {
        }

        private static string StaticFunction()
        {
            return "hello";
        }

        private static string StaticFunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
        {
            return "hello";
        }

        private static string StaticFunctionWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17)
        {
            return "hello";
        }

        private void Procedure()
        {
        }

        private string Function()
        {
            return "hello";
        }

        private void Procedure(int a)
        {
        }

        private string Function(int a)
        {
            return a.ToString();
        }

        private void Procedure(int a, OtherType otherType)
        {
        }

        private string Function(int a, OtherType otherType)
        {
            return $"{a}, {otherType.Number}";
        }

        private void ProcedureWith15Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15)
        {
        }

        private void ProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
        {
        }

        private string FunctionWith15Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15)
        {
            return "hello";
        }

        private string FunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
        {
            return "hello";
        }
    }
}