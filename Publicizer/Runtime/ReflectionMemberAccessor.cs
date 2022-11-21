// NOTE: This file will be included in the receiver project as source code, so we disable nullable warning context when used from the receiver project,
// because nullable behavior changes too frequently between different .NET versions, and we do not want this code to fail at compile time due to nullable problems.
#if !NULLABLE_CHECK_FOR_INCLUDED_CODE
#nullable enable annotations
#nullable disable warnings
#endif

using System.Reflection;

namespace Publicizer.Runtime
{
    /// <summary>
    /// Member accessor logic which uses reflection.
    /// </summary>
    /// <typeparam name="T">The type which has the members.</typeparam>
    /// <remarks>
    /// It can be used by the forwarding code of the generated public members inside the proxy class to access the (typically private) members
    /// of the original class (<typeparamref name="T"/>).
    /// </remarks>
    internal class ReflectionMemberAccessor<T> : IMemberAccessor<T>
    {
        /// <inheritdoc />
#nullable disable warnings
        public TField GetValue<TField>(FieldInfo field, T? instance) => (TField)field.GetValue(instance);
#nullable restore warnings

        /// <inheritdoc />
        public void SetValue<TField>(FieldInfo field, T? instance, TField value) => field.SetValue(instance, value);

        /// <inheritdoc />
#nullable disable warnings
        public TProperty GetValue<TProperty>(PropertyInfo property, T? instance) => (TProperty)property.GetValue(instance);
#nullable restore warnings

        /// <inheritdoc />
        public void SetValue<TProperty>(PropertyInfo property, T? instance, TProperty value) => property.SetValue(instance, value);

        /// <inheritdoc />
        public object? InvokeMethod(MethodInfo method, T? instance, object[] parameterValues) => method.Invoke(instance, parameterValues);
    }
}
