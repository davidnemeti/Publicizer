using System;

namespace Publicizer
{
    /// <summary>
    /// The member accessor logic to access the members of a type during runtime.
    /// </summary>
    /// <typeparam name="T">The type which has the members.</typeparam>
    /// <remarks>
    /// It is used by the forwarding code of the generated public members inside the proxy class to access the (typically private) members of the original class (<typeparamref name="T"/>).
    /// </remarks>
    public interface IMemberAccessor<T>
    {
        /// <summary>
        /// Gets the value of a field.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="instance">The instance of the type which contains the field. It is <c>null</c> if the field is static.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="fieldLifetime">The lifetime of the field.</param>
        /// <param name="fieldVisibility">The visibility of the field.</param>
        /// <returns>The value of the field.</returns>
        TField GetFieldValue<TField>(T? instance, string fieldName, MemberLifetime fieldLifetime, MemberVisibility fieldVisibility);

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="instance">The instance of the type which contains the field. It is <c>null</c> if the field is static.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="fieldValue">The new value of the field.</param>
        /// <param name="fieldLifetime">The lifetime of the field.</param>
        /// <param name="fieldVisibility">The visibility of the field.</param>
        void SetFieldValue<TField>(T? instance, string fieldName, TField fieldValue, MemberLifetime fieldLifetime, MemberVisibility fieldVisibility);

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="instance">The instance of the type which contains the property. It is <c>null</c> if the property is static.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyLifetime">The lifetime of the property.</param>
        /// <param name="propertyVisibility">The visibility of the property.</param>
        /// <returns>The value of the property.</returns>
        TProperty GetPropertyValue<TProperty>(T? instance, string propertyName, MemberLifetime propertyLifetime, MemberVisibility propertyVisibility);

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="instance">The instance of the type which contains the property. It is <c>null</c> if the property is static.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The new value of the property.</param>
        /// <param name="propertyLifetime">The lifetime of the property.</param>
        /// <param name="propertyVisibility">The visibility of the property.</param>
        void SetPropertyValue<TProperty>(T? instance, string propertyName, TProperty propertyValue, MemberLifetime propertyLifetime, MemberVisibility propertyVisibility);

        /// <summary>
        /// Invokes a method.
        /// </summary>
        /// <param name="instance">The instance of the type which contains the method. It is <c>null</c> if the method is static.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameterTypes">The types of the parameters of the method.</param>
        /// <param name="parameterValues">The values of the parameters of the method.</param>
        /// <param name="methodLifetime">The lifetime of the method.</param>
        /// <param name="methodVisibility">The visibility of the method.</param>
        /// <returns></returns>
        object? InvokeMethod(T? instance, string methodName, Type[] parameterTypes, object[] parameterValues, MemberLifetime methodLifetime, MemberVisibility methodVisibility);
    }
}
