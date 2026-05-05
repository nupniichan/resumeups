namespace resumeups.Server.Utils;

internal static class LlmTextHelper
{
    internal static string CleanJsonResponse(string text)
    {
        text = text.Trim();
        if (text.StartsWith("```json", StringComparison.Ordinal))
            text = text[7..];
        else if (text.StartsWith("```", StringComparison.Ordinal))
            text = text[3..];
        if (text.EndsWith("```", StringComparison.Ordinal))
            text = text[..^3];
        return text.Trim();
    }
}
