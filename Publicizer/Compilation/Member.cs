using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Publicizer.Compilation;

internal abstract class Member
{
    public ISymbol Symbol { get; }

    protected Member(ISymbol symbol)
    {
        Symbol = symbol;
    }

    public ITypeSymbol ContainingType => Symbol.ContainingType;
    public string ContainingTypeFullName => ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    public string Name => Symbol.Name;
    public string FullName => $"{ContainingTypeFullName}.{Name}";
    public bool IsStatic => Symbol.IsStatic;

    protected static string GetTypeFullName(ITypeSymbol type) => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    protected static string GetTypeFullNameWithNullableSupport(ITypeSymbol type)
    {
        var typeFullName = GetTypeFullName(type);

        return type.NullableAnnotation == NullableAnnotation.Annotated && !typeFullName.EndsWith("?")
            ? $"{typeFullName}?"
            : typeFullName;
    }
}

internal sealed class Method : Member
{
    public Method(IMethodSymbol symbol)
        : base(symbol)
    {
    }

    public new IMethodSymbol Symbol => (IMethodSymbol)base.Symbol;

    public ITypeSymbol ReturnType => Symbol.ReturnType;
    public bool ReturnsVoid => Symbol.ReturnsVoid;
    public string ReturnTypeFullName => GetTypeFullName(ReturnType);
    public string ReturnTypeFullNameWithNullableSupport => GetTypeFullNameWithNullableSupport(ReturnType);

    public IEnumerable<string> GetParameterTypesFullNames() => Symbol.Parameters.Select(parameter => GetTypeFullName(parameter.Type));
    public IEnumerable<string> GetParameterTypesFullNamesWithNullableSupport() => Symbol.Parameters.Select(parameter => GetTypeFullNameWithNullableSupport(parameter.Type));
    public IEnumerable<string> GetParameterNames() => Symbol.Parameters.Select(parameter => parameter.Name);
}

internal abstract class Value : Member
{
    private Value(ISymbol symbol)
        : base(symbol)
    {
    }

    public abstract ITypeSymbol Type { get; }
    public abstract bool CanRead { get; }
    public abstract bool CanWrite { get; }

    public string TypeFullName => GetTypeFullName(Type);
    public string TypeFullNameWithNullableSupport => GetTypeFullNameWithNullableSupport(Type);

    public TResult Select<TResult>(Func<Field, TResult> fieldSelector, Func<Property, TResult> propertySelector) =>
        this switch
        {
            Field field => fieldSelector(field),
            Property property => propertySelector(property),
            _ => throw new InvalidOperationException($"{typeof(Field)} or {typeof(Property)} is needed instead of '{GetType()}'")
        };

    internal sealed class Field : Value
    {
        public Field(IFieldSymbol symbol)
            : base(symbol)
        {
        }

        public new IFieldSymbol Symbol => (IFieldSymbol)base.Symbol;
        public override ITypeSymbol Type => Symbol.Type;
        public override bool CanRead => true;
        public override bool CanWrite => !Symbol.IsReadOnly;
    }

    internal sealed class Property : Value
    {
        public Field? BackingField { get; }

        public Property(IPropertySymbol symbol, IReadOnlyDictionary<IPropertySymbol, IFieldSymbol> propertyToBackingField)
            : base(symbol)
        {
            BackingField = propertyToBackingField.TryGetValue(symbol, out var backingFieldSymbol)
                ? new Field(backingFieldSymbol)
                : null;
        }

        public new IPropertySymbol Symbol => (IPropertySymbol)base.Symbol;
        public override ITypeSymbol Type => Symbol.Type;
        public override bool CanRead => Symbol.GetMethod is not null;
        public override bool CanWrite => Symbol.SetMethod is not null;
    }
}
