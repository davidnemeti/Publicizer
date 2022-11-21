using System.Reflection;
using Publicizer.Runtime;

namespace Publicizer.Tests;

internal static class ReflectionMemberAccessorExtensions
{
    private const BindingFlags s_bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public static void SetFieldValue<T>(this ReflectionMemberAccessor<T> reflectionMemberAccessor, T? instance, string fieldName, object? value)
    {
        var field = typeof(T).GetField(fieldName, s_bindingFlags) ?? throw new ArgumentException($"Field '{fieldName}' not found in type {typeof(T)}", nameof(fieldName));
        reflectionMemberAccessor.SetValue(field, instance, value);
    }

    public static void SetPropertyValue<T>(this ReflectionMemberAccessor<T> reflectionMemberAccessor, T? instance, string propertyName, object? value)
    {
        var property = typeof(T).GetProperty(propertyName, s_bindingFlags) ?? throw new ArgumentException($"Property '{propertyName}' not found in type {typeof(T)}", nameof(propertyName));
        reflectionMemberAccessor.SetValue(property, instance, value);
    }
}
