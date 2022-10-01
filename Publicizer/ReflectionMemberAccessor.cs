using System;

namespace Publicizer
{
    /// <summary>
    /// Member accessor logic which uses reflection.
    /// </summary>
    /// <typeparam name="T">The type which has the members.</typeparam>
    /// <remarks>
    /// It is the default member accessor logic, if no custom member accessor is given.
    /// It is used by the forwarding code of the generated public members inside the proxy class to access the (typically private) members of the original class (<typeparamref name="T"/>).
    /// </remarks>
    public class ReflectionMemberAccessor<T> : IMemberAccessor<T>
    {
        /// <inheritdoc />
        public TField GetFieldValue<TField>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            (TField)typeof(T)
                .GetField(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .GetValue(instance);

        /// <inheritdoc />
        public void SetFieldValue<TField>(T? instance, string fieldName, TField fieldValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            typeof(T)
                .GetField(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .SetValue(instance, fieldValue);

        /// <inheritdoc />
        public TProperty GetPropertyValue<TProperty>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            (TProperty)typeof(T)
                .GetProperty(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .GetValue(instance);

        /// <inheritdoc />
        public void SetPropertyValue<TProperty>(T? instance, string fieldName, TProperty propertyValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            typeof(T)
                .GetProperty(fieldName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags())
                .SetValue(instance, propertyValue);

        /// <inheritdoc />
        public object? InvokeMethod(T? instance, string methodName, Type[] parameterTypes, object[] parameterValues, MemberLifetime memberLifetime, MemberVisibility memberVisibility) =>
            typeof(T)
                .GetMethod(methodName, memberLifetime.ToBindingFlags() | memberVisibility.ToBindingFlags(), binder: null, parameterTypes, modifiers: null)
                .Invoke(instance, parameterValues);
    }
}
