using System.Text.Json;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;

namespace resumeups.Server.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly ILLMClient _llmClient;

        public MatchingService(ILLMClient llmClient)
        {
            _llmClient = llmClient;
        }

        public async Task<MatchingResult> AnalyzeMatching(string resume, string jd)
        {
            var prompt = SystemPrompt.MatchingSystemPrompt
                .Replace("{RESUME}", resume)
                .Replace("{JD}", jd);

            var messages = new List<LlmChatMessage>
            {
                new("user", prompt)
            };

            var contentText = await _llmClient.GetResponseAsync(messages, 0);

            contentText = LlmTextHelper.CleanJsonResponse(contentText);

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
    }
}
