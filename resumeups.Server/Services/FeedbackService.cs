using System.Text.Json;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;

namespace resumeups.Server.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ILLMClient _llmClient;

        public FeedbackService(ILLMClient llmClient)
        {
            _llmClient = llmClient;
        }

        public async Task<FeedbackResult> AnalyzeFeedback(string resume, string jobDescription, MatchingResult matching)
        {
            var prompt = SystemPrompt.FeedbackSystemPrompt
                .Replace("{RESUME}", resume)
                .Replace("{JD}", jobDescription)
                .Replace("{MATCH_SCORE}", matching.MatchScore.ToString())
                .Replace("{KEYWORDS_MATCHING}", string.Join(", ", matching.KeywordsMatching))
                .Replace("{KEYWORDS_MISSING}", string.Join(", ", matching.KeywordsMissing));

            var messages = new List<LlmChatMessage>
            {
                new("user", prompt)
            };

            var contentText = await _llmClient.GetResponseAsync(messages, 0);

            contentText = LlmTextHelper.CleanJsonResponse(contentText);

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

                    issues.Add($"[{severity}] [{category}] {issue}");
                }
            }

            if (root.TryGetProperty("suggestions", out var suggestionsArray))
            {
                foreach (var item in suggestionsArray.EnumerateArray())
                {
                    var severity = item.TryGetProperty("severity", out var sev) ? sev.GetString() : "";
                    var suggestion = item.TryGetProperty("suggestion", out var sug) ? sug.GetString() : "";

                    suggestions.Add($"[{severity}] {suggestion}");
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
