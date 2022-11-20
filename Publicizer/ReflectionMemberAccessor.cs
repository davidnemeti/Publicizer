using System.Reflection;

namespace Publicizer
{
    /// <summary>
    /// Member accessor logic which uses reflection.
    /// </summary>
    /// <typeparam name="T">The type which has the members.</typeparam>
    /// <remarks>
    /// It can be used by the forwarding code of the generated public members inside the proxy class to access the (typically private) members
    /// of the original class (<typeparamref name="T"/>).
    /// </remarks>
    public class ReflectionMemberAccessor<T> : IMemberAccessor<T>
    {
        /// <inheritdoc />
        public TField GetValue<TField>(FieldInfo field, T? instance) => (TField) field.GetValue(instance);

        /// <inheritdoc />
        public void SetValue<TField>(FieldInfo field, T? instance, TField value) => field.SetValue(instance, value);

        /// <inheritdoc />
        public TProperty GetValue<TProperty>(PropertyInfo property, T? instance) => (TProperty) property.GetValue(instance);

        /// <inheritdoc />
        public void SetValue<TProperty>(PropertyInfo property, T? instance, TProperty value) => property.SetValue(instance, value);

        /// <inheritdoc />
        public object? InvokeMethod(MethodInfo method, T? instance, object[] parameterValues) => method.Invoke(instance, parameterValues);
    }
}
