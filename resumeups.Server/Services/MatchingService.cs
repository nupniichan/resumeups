using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;

namespace resumeups.Server.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MatchingService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MatchingResult> AnalyzeMatching(string resume, string jd)
        {
            var apiKey = EnvReader.Get("OPENROUTER_API_KEY");
            var model = EnvReader.Get("LLM_MODEL", "tencent/hy3-preview:free");

            var prompt = SystemPrompt.MatchingSystemPrompt
                .Replace("{RESUME}", resume)
                .Replace("{JD}", jd);

            var requestBody = new
            {
                model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.0
            };

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://openrouter.ai/api/v1/chat/completions", jsonContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            var contentText = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";

            contentText = CleanJsonResponse(contentText);

            using var resultDoc = JsonDocument.Parse(contentText);
            var root = resultDoc.RootElement;

            var keywordsMatching = root.GetProperty("keywords_matching")
                .EnumerateArray()
                .Select(e => e.GetString() ?? "")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var keywordsMissing = root.GetProperty("keywords_missing")
                .EnumerateArray()
                .Select(e => e.GetString() ?? "")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var matchScore = CalculateMatchScore(keywordsMatching.Count, keywordsMissing.Count);

            var result = new MatchingResult
            {
                MatchScore = matchScore,
                KeywordsMatching = keywordsMatching,
                KeywordsMissing = keywordsMissing
            };

            return result;
        }

        private static int CalculateMatchScore(int matchedCount, int missingCount)
        {
            var total = matchedCount + missingCount;
            if (total == 0)
                return 0;

            var score = (double)matchedCount / total * 100;
            return (int)Math.Round(score, MidpointRounding.AwayFromZero);
        }

        private static string CleanJsonResponse(string text)
        {
            text = text.Trim();
            if (text.StartsWith("```json"))
                text = text[7..];
            else if (text.StartsWith("```"))
                text = text[3..];
            if (text.EndsWith("```"))
                text = text[..^3];
            return text.Trim();
        }
    }
}
