using System;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Publicizer
{
    internal class Namer
    {
        private static readonly Regex s_illegalCharacterMatcher = new Regex(@"[:.<>]");

        private readonly INamedTypeSymbol _proxyTypeSymbol;
        private readonly INamedTypeSymbol _typeSymbolToPublicize;

        public string TypeToPublicizeFullName { get; }
        public string? MemberAccessorTypeText { get; }
        public string? MemberAccessorInstanceText { get; }
        public string? InstanceToPublicizeName { get; }

        public Namer(INamedTypeSymbol proxyTypeSymbol, INamedTypeSymbol typeSymbolToPublicize, INamedTypeSymbol? customMemberAccessorTypeSymbol, MemberLifetime memberLifetime)
        {
            _proxyTypeSymbol = proxyTypeSymbol;
            _typeSymbolToPublicize = typeSymbolToPublicize;
            TypeToPublicizeFullName = typeSymbolToPublicize.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            MemberAccessorTypeText = customMemberAccessorTypeSymbol is not null
                ? customMemberAccessorTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                : null;

            MemberAccessorInstanceText = customMemberAccessorTypeSymbol is not null
                ? ToGeneratedName(nameof(IMemberAccessor<object>))
                : null;

            InstanceToPublicizeName = memberLifetime.HasFlag(MemberLifetime.Instance)
                ? ToGeneratedName(_typeSymbolToPublicize.Name)
                : null;
        }

        public string GetInstanceText(Member member) =>
            member.IsStatic
                ? "null"
                : $"this.{InstanceToPublicizeName ?? throw new InvalidOperationException($"{nameof(InstanceToPublicizeName)} is null")}";

        public string GetValueInfoName(Value value)
        {
            var memberText = value.Select(field => "Field", property => "Property");
            return ToGeneratedName($"{memberText}Info_{value.Name}");
        }

        public string GetMethodInfoName(Method method) => ToGeneratedName($"MethodInfo_{method.Name}_" + string.Join("_", method.GetParameterTypesFullNames()));

        public string GetValueGetterName(Value value)
        {
            var memberText = value.Select(field => "Field", property => "Property");
            return ToGeneratedName($"Get{memberText}_{value.Name}");
        }

        public string GetValueSetterName(Value value)
        {
            var memberText = value.Select(field => "Field", property => "Property");
            return ToGeneratedName($"Set{memberText}_{value.Name}");
        }

        public string GetInvokeName(Method method) => ToGeneratedName($"InvokeMethod_{method.Name}_" + string.Join("_", method.GetParameterTypesFullNames()));

        private string ToGeneratedName(string name) => s_illegalCharacterMatcher.Replace($"__{_proxyTypeSymbol.Name}_{name}", "_");
    }
}
