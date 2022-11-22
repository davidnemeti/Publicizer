using NamespaceForOtherTypes;

namespace Publicizer.Tests;

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
