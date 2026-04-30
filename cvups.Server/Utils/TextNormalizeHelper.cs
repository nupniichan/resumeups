namespace cvups.Server.Utils
{
    public class TextNormalizeHelper
    {
        public static string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var text = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"(\d+)\.\s+(\d+)", "$1.$2");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\.\s+([a-zA-Z])", ".$1");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+([,.!?;:])", "$1");

            return text.Trim();
        }
    }
}