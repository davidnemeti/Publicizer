using Publicizer;

public class SpecialMemberAccessor<T> : IMemberAccessor<T>
{
    private readonly ReflectionMemberAccessor<T> _reflectionMemberAccessor = new ReflectionMemberAccessor<T>();

    public TField GetFieldValue<TField>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        return _reflectionMemberAccessor.GetFieldValue<TField>(instance, fieldName, memberLifetime, memberVisibility);
    }

    public TProperty GetPropertyValue<TProperty>(T? instance, string fieldName, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        return _reflectionMemberAccessor.GetPropertyValue<TProperty>(instance, fieldName, memberLifetime, memberVisibility);
    }

    public object? InvokeMethod(T? instance, string methodName, Type[] parameterTypes, object[] parameterValues, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        return _reflectionMemberAccessor.InvokeMethod(instance, methodName, parameterTypes, parameterValues, memberLifetime, memberVisibility);
    }

    public void SetFieldValue<TField>(T? instance, string fieldName, TField fieldValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        _reflectionMemberAccessor.SetFieldValue(instance, fieldName, fieldValue, memberLifetime, memberVisibility);
    }

    public void SetPropertyValue<TProperty>(T? instance, string fieldName, TProperty propertyValue, MemberLifetime memberLifetime, MemberVisibility memberVisibility)
    {
        _reflectionMemberAccessor.SetPropertyValue(instance, fieldName, propertyValue, memberLifetime, memberVisibility);
    }
}
