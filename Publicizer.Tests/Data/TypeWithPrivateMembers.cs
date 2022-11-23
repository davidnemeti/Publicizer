﻿using Publicizer.Tests.Helpers;
using NamespaceForOtherTypes;

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

    private static void StaticProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        StaticLogger.Log();
    }

    private static void StaticProcedureWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17)
    {
        StaticLogger.Log();
    }

    private static string StaticFunction()
    {
        StaticLogger.Log();
        return "hello";
    }

    private static int StaticFunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        StaticLogger.Log();
        return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16;
    }

    private static int StaticFunctionWith17Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16, int a17)
    {
        StaticLogger.Log();
        return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16 + a17;
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

    private void ProcedureWith15Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15)
    {
        StaticLogger.Log();
    }

    private void ProcedureWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        StaticLogger.Log();
    }

    private int FunctionWith15Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15)
    {
        StaticLogger.Log();
        return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15;
    }

    private int FunctionWith16Parameters(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, int a16)
    {
        StaticLogger.Log();
        return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16;
    }
}
