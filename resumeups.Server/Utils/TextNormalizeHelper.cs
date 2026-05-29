using System.Text.RegularExpressions;

namespace resumeups.Server.Utils
{
    public static partial class TextNormalizeHelper
    {
        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();

        [GeneratedRegex(@"(\d+)\.\s+(\d+)")]
        private static partial Regex DecimalSpacingRegex();

        [GeneratedRegex(@"\.\s+([a-zA-Z])")]
        private static partial Regex SentenceSpacingRegex();

        [GeneratedRegex(@"\s+([,.!?;:])")]
        private static partial Regex PunctuationSpacingRegex();

        public static string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var text = WhitespaceRegex().Replace(input, " ");
            text = DecimalSpacingRegex().Replace(text, "$1.$2");
            text = SentenceSpacingRegex().Replace(text, ".$1");
            text = PunctuationSpacingRegex().Replace(text, "$1");

            return text.Trim();
        }
    }
}