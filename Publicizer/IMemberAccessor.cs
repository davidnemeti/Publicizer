using System;

namespace Publicizer
{
    public interface IMemberAccessor<T>
    {
        TField GetFieldValue<TField>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility);
        void SetFieldValue<TField>(T? instance, string fieldName, TField fieldValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility);

        TProperty GetPropertyValue<TProperty>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility);
        void SetPropertyValue<TProperty>(T? instance, string fieldName, TProperty propertyValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility);

        object? InvokeMethod(T? instance, string methodName, Type[] parameterTypes, object[] parameterValues, MemberLifetime memberLifetime, MemberVisibility memberVisibility);
    }
}
