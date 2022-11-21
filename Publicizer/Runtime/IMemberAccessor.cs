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
    /// The member accessor logic to access the members of a type during runtime.
    /// </summary>
    /// <typeparam name="T">The type which has the members.</typeparam>
    /// <remarks>
    /// It is used by the forwarding code of the generated public members inside the proxy class to access the (typically private) members of the original class (<typeparamref name="T"/>).
    /// </remarks>
    internal interface IMemberAccessor<T>
    {
        /// <summary>
        /// Gets the value of a field.
        /// </summary>
        /// <typeparam name="TField">The type of <paramref name="field"/>.</typeparam>
        /// <param name="field">The field of <typeparamref name="T"/>.</param>
        /// <param name="instance">The instance of <typeparamref name="T"/> which contains <paramref name="field"/>. It is <c>null</c> if the field is static.</param>
        /// <returns>The value of the <paramref name="field"/>.</returns>
        TField GetValue<TField>(FieldInfo field, T? instance);

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <typeparam name="TField">The type of <paramref name="field"/>.</typeparam>
        /// <param name="field">The field of <typeparamref name="T"/>.</param>
        /// <param name="instance">The instance of <typeparamref name="T"/> which contains <paramref name="field"/>. It is <c>null</c> if the field is static.</param>
        /// <param name="value">The new value of <paramref name="field"/>.</param>
        void SetValue<TField>(FieldInfo field, T? instance, TField value);

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of <paramref name="property"/>.</typeparam>
        /// <param name="property">The property of <typeparamref name="T"/>.</param>
        /// <param name="instance">The instance of <typeparamref name="T"/> which contains <paramref name="property"/>. It is <c>null</c> if the property is static.</param>
        /// <returns>The value of the <paramref name="property"/>.</returns>
        TProperty GetValue<TProperty>(PropertyInfo property, T? instance);

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of <paramref name="property"/>.</typeparam>
        /// <param name="property">The field of <typeparamref name="T"/>.</param>
        /// <param name="instance">The instance of <typeparamref name="T"/> which contains <paramref name="property"/>. It is <c>null</c> if the property is static.</param>
        /// <param name="value">The new value of <paramref name="property"/>.</param>
        void SetValue<TProperty>(PropertyInfo property, T? instance, TProperty value);

        /// <summary>
        /// Invokes a method.
        /// </summary>
        /// <param name="method">The method of <typeparamref name="T"/>.</param>
        /// <param name="instance">The instance of <typeparamref name="T"/> which contains <paramref name="method"/>. It is <c>null</c> if the method is static.</param>
        /// <param name="parameterValues">The values of the parameters of <paramref name="method"/>.</param>
        /// <returns></returns>
        object? InvokeMethod(MethodInfo method, T? instance, object[] parameterValues);
    }
}
