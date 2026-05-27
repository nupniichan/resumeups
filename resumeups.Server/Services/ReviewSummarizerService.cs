using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;

namespace resumeups.Server.Services
{
    public sealed class ReviewSummarizerService : IReviewSummarizerService
    {
        private readonly ILLMClient _llmClient;

        public ReviewSummarizerService(ILLMClient llmClient)
        {
            _llmClient = llmClient;
        }

        public async Task<MultiSiteMatchResult> MatchCompaniesAsync(
            string companyName,
            Note8CandidateData note8Candidates,
            ReviewCongTyCandidateData rctCandidates)
        {
            var prompt = CompanyReviewsPrompt.MultiSiteMatchPrompt
                .Replace("{SEARCH_QUERY}", companyName)
                .Replace("{NOTE8_CANDIDATES}", note8Candidates.HasResults ? note8Candidates.CandidatesJson : "[]")
                .Replace("{REVIEWCONGTY_CANDIDATES}", rctCandidates.HasResults ? rctCandidates.CandidatesJson : "[]");

            var responseText = await _llmClient.GetResponseAsync(new[] { new LlmChatMessage("user", prompt) }, 0);
            responseText = LlmTextHelper.CleanJsonResponse(responseText);

            using var doc = JsonDocument.Parse(responseText);
            return new MultiSiteMatchResult
            {
                Note8Slug = doc.RootElement.GetProperty("note8Slug").GetString() ?? "NOT_FOUND",
                ReviewCongTySlug = doc.RootElement.GetProperty("reviewCongTySlug").GetString() ?? "NOT_FOUND"
            };
        }

        public async Task<(ReviewStats note8, ReviewStats reviewCongTy)> SummarizeBothAsync(
            List<string> note8Reviews,
            List<string> rctReviews)
        {
            var note8Blob = note8Reviews.Count > 0 ? string.Join("\n---\n", note8Reviews) : "NO_REVIEWS";
            var rctBlob = rctReviews.Count > 0 ? string.Join("\n---\n", rctReviews) : "NO_REVIEWS";

            var prompt = CompanyReviewsPrompt.MultiSiteSummarizationPrompt
                .Replace("{NOTE8_REVIEWS}", note8Blob)
                .Replace("{REVIEWCONGTY_REVIEWS}", rctBlob);

            try
            {
                var responseText = await _llmClient.GetResponseAsync(new[] { new LlmChatMessage("user", prompt) }, 0);
                responseText = LlmTextHelper.CleanJsonResponse(responseText);

                using var doc = JsonDocument.Parse(responseText);
                var note8Summary = ParseSummary(doc.RootElement.GetProperty("note8"));
                var rctSummary = ParseSummary(doc.RootElement.GetProperty("reviewCongTy"));
                return (note8Summary, rctSummary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AI review summarization: {ex.Message}");
                var fallback = FallbackSummary();
                return (fallback, FallbackSummary());
            }
        }

        private static ReviewStats ParseSummary(JsonElement el)
        {
            var summary = el.GetProperty("summary").GetString() ?? "";
            var pros = new List<string>();
            var cons = new List<string>();
            var recs = new List<string>();

            foreach (var x in el.GetProperty("pros").EnumerateArray())
                pros.Add(x.GetString() ?? "");
            foreach (var x in el.GetProperty("cons").EnumerateArray())
                cons.Add(x.GetString() ?? "");
            foreach (var x in el.GetProperty("recommendations").EnumerateArray())
                recs.Add(x.GetString() ?? "");

            return new ReviewStats
            {
                Summary = summary,
                Pros = pros,
                Cons = cons,
                Recommendations = recs
            };
        }

        private static ReviewStats FallbackSummary() => new()
        {
            Summary = "Đã xảy ra lỗi khi tổng hợp nhận xét từ AI.",
            Pros = new List<string> { "Lỗi tổng hợp" },
            Cons = new List<string> { "Lỗi tổng hợp" },
            Recommendations = new List<string> { "Vui lòng xem chi tiết ở các đánh giá ví dụ bên dưới" }
        };
    }
}
