using System.Linq;
using Microsoft.CodeAnalysis;

namespace Publicizer.Compilation
{
    internal static class CodeAnalysisExtensions
    {
        public static T GetPublicizeAttributeNamedArgumentValue<T>(this AttributeData publicizeAttributeData, string argumentName, T defaultValue)
            where T : notnull
        {
            var foundNamedArgumentOrNull = GetNamedArgumentOrNull(publicizeAttributeData, argumentName);

            return foundNamedArgumentOrNull is not null
                ? (T)foundNamedArgumentOrNull.Value.Value!
                : defaultValue;
        }

        public static T? GetPublicizeAttributeNamedArgumentValue<T>(this AttributeData publicizeAttributeData, string argumentName)
            where T : notnull
        {
            var foundNamedArgumentOrNull = GetNamedArgumentOrNull(publicizeAttributeData, argumentName);

            return foundNamedArgumentOrNull is not null
                ? (T?)foundNamedArgumentOrNull.Value.Value
                : default;
        }

        private static TypedConstant? GetNamedArgumentOrNull(AttributeData publicizeAttributeData, string argumentName)
        {
            var foundNamedArgument = publicizeAttributeData.NamedArguments.SingleOrDefault(namedArgument => namedArgument.Key == argumentName);

            return foundNamedArgument.Key is not null
                ? foundNamedArgument.Value
                : null;
        }
    }
}
