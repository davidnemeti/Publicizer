using System.Linq;
using Microsoft.CodeAnalysis;

namespace Publicizer
{
    internal static class CodeAnalysisExtensions
    {
        private static readonly PublicizeAttribute _publicizeAttribute = new (typeof(CodeAnalysisExtensions));

        public static object? GetPublicizeAttributeNamedArgumentValue(this AttributeData publicizeAttributeData, string argumentName)
        {
            var foundNamedArgument = publicizeAttributeData.NamedArguments.SingleOrDefault(namedArgument => namedArgument.Key == argumentName);

            return foundNamedArgument.Key is not null
                ? foundNamedArgument.Value.Value
                : typeof(PublicizeAttribute).GetProperty(argumentName).GetValue(_publicizeAttribute);
        }
    }
}
