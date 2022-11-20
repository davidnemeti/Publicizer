using System.CodeDom.Compiler;
using System.Linq;
using System.Text.RegularExpressions;

namespace Publicizer
{
    internal static class IndentedTextWriterExtensions
    {
        // unfortunately, IndentedTextWriter._tabString is private, so we cannot use it to get the actual tabstring automatically
        // (maybe we should use Publicizer to access the private field :-) )
        public static void WriteMultiLine(this IndentedTextWriter indentedWriter, string multiLineText, string tabString = IndentedTextWriter.DefaultTabString)
        {
            multiLineText = GetIndentedMultiLineText(multiLineText, indentedWriter.Indent, tabString, indentedWriter.NewLine);
            indentedWriter.WriteLineNoTabs(multiLineText);
        }

        private static string GetIndentedMultiLineText(string multiLineText, int indent, string tabString, string newLine)
        {
            var indentText = string.Join(string.Empty, Enumerable.Repeat(tabString, indent));
            return indentText + Regex.Replace(multiLineText, newLine, newLine + indentText);
        }
    }
}
