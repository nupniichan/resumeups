using System;
using System.Text.RegularExpressions;

namespace resumeups.Server.Services.Scrapers
{
    public static class GlassdoorReviewParser
    {
        public static (string? slug, string? host) ExtractGlassdoorSlugAndHost(string html)
        {
            if (string.IsNullOrEmpty(html)) return (null, null);

            var match = Regex.Match(html, @"([a-z0-9.-]*glassdoor\.[a-z0-9.]+)(?:\/|%2[fF])Reviews(?:\/|%2[fF])([^/&?#""\s\)\>%]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var host = match.Groups[1].Value;
                var slug = match.Groups[2].Value;
                return (Uri.UnescapeDataString(slug), host);
            }
            return (null, null);
        }
    }
}
