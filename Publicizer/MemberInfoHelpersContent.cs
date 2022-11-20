using System.Reflection;
using System.Reflection.Emit;

namespace Publicizer
{
    internal static class MemberInfoHelpersContent
    {
        public static TDelegate CreateSetActionByEmittingIL<TDelegate>(FieldInfo fieldInfo)
            where TDelegate : Delegate
        {
            // NOTE: this method works for setting readonly fields too

            // TODO: create one, single assembly for all of the generated methods (should be passed as parameter to CreateSetActionByEmittingIL)
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicMethodAssembly"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            // Set restrictedSkipVisibility to true to avoid any pesky "visibility" checks being made (in other
            // words, let the IL in the generated method access any private types or members that it tries to)
            var method = new DynamicMethod(
                name: "SetField",
                returnType: null,
                parameterTypes: fieldInfo.IsStatic
                    ? new Type[] { fieldInfo.FieldType }
                    : new Type[] { fieldInfo.ReflectedType!, fieldInfo.FieldType },
                moduleBuilder,
                skipVisibility: true
            );

            var gen = method.GetILGenerator();
            if (fieldInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldnull);
                gen.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
            }
            gen.Emit(OpCodes.Stfld, fieldInfo);
            gen.Emit(OpCodes.Ret);

            return method.CreateDelegate<TDelegate>();
        }
    }
}
