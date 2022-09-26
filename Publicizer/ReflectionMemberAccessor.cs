using System;

namespace Publicizer
{
    public class ReflectionMemberAccessor<T> : IMemberAccessor<T>
    {
        public TField GetFieldValue<TField>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            (TField)typeof(T)
                .GetField(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .GetValue(instance);

        public void SetFieldValue<TField>(T? instance, string fieldName, TField fieldValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            typeof(T)
                .GetField(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .SetValue(instance, fieldValue);

        public TProperty GetPropertyValue<TProperty>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            (TProperty)typeof(T)
                .GetProperty(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .GetValue(instance);

        public void SetPropertyValue<TProperty>(T? instance, string fieldName, TProperty propertyValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            typeof(T)
                .GetProperty(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .SetValue(instance, propertyValue);

        public object? InvokeMethod(T? instance, string methodName, Type[] parameterTypes, object[] parameterValues, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            typeof(T)
                .GetMethod(methodName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags(), binder: null, parameterTypes, modifiers: null)
                .Invoke(instance, parameterValues);
    }
}
