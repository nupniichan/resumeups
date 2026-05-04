using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;

namespace resumeups.Server.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FeedbackService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<FeedbackResult> AnalyzeFeedback(string resume, string jobDescription, MatchingResult matching)
        {
            var apiKey = EnvReader.Get("OPENROUTER_API_KEY");
            var model = EnvReader.Get("LLM_MODEL", "tencent/hy3-preview:free");

            var prompt = SystemPrompt.FeedbackSystemPrompt
                .Replace("{RESUME}", resume)
                .Replace("{JD}", jobDescription)
                .Replace("{MATCH_SCORE}", matching.MatchScore.ToString())
                .Replace("{KEYWORDS_MATCHING}", string.Join(", ", matching.KeywordsMatching))
                .Replace("{KEYWORDS_MISSING}", string.Join(", ", matching.KeywordsMissing));

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

            var issues = new List<string>();
            var suggestions = new List<string>();
            var contextScore = ReadInt(root, "context_score");
            var impactScore = ReadInt(root, "impact_score");

            if (root.TryGetProperty("issues", out var issuesArray))
            {
                foreach (var item in issuesArray.EnumerateArray())
                {
                    var category = item.TryGetProperty("category", out var cat) ? cat.GetString() : "";
                    var severity = item.TryGetProperty("severity", out var sev) ? sev.GetString() : "";
                    var issue = item.TryGetProperty("issue", out var iss) ? iss.GetString() : "";
                    var before = item.TryGetProperty("before", out var bef) ? bef.GetString() : "";
                    var after = item.TryGetProperty("after", out var aft) ? aft.GetString() : "";

                    issues.Add($"[{severity}] [{category}] {issue}");
                    if (!string.IsNullOrWhiteSpace(before) || !string.IsNullOrWhiteSpace(after))
                        suggestions.Add($"[{category}] Before: {before} -> After: {after}");
                }
            }

            var feedbackScore = CalculateFeedbackScore(matching.MatchScore, contextScore, impactScore);

            var result = new FeedbackResult
            {
                FeedbackScore = feedbackScore,
                Summary = root.GetProperty("summary").GetString() ?? "",
                Issues = issues,
                Suggestions = suggestions
            };

            return result;
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

        private static int ReadInt(JsonElement root, string propertyName)
        {
            if (root.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.Number)
                return Math.Clamp(value.GetInt32(), 0, 100);

            return 0;
        }

        private static int CalculateFeedbackScore(int matchScore, int contextScore, int impactScore)
        {
            var weightedScore = matchScore * 0.5 + contextScore * 0.25 + impactScore * 0.25;
            return (int)Math.Round(weightedScore, MidpointRounding.AwayFromZero);
        }
    }
}
