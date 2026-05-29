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
            List<string> rctReviews,
            string language = "vi")
        {
            var note8Blob = note8Reviews.Count > 0 ? string.Join("\n---\n", note8Reviews) : "NO_REVIEWS";
            var rctBlob = rctReviews.Count > 0 ? string.Join("\n---\n", rctReviews) : "NO_REVIEWS";

            var translation = GetTranslation(language);
            var prompt = CompanyReviewsPrompt.MultiSiteSummarizationPrompt
                .Replace("{NOTE8_REVIEWS}", note8Blob)
                .Replace("{REVIEWCONGTY_REVIEWS}", rctBlob)
                .Replace("{LANGUAGE}", translation.LanguageName)
                .Replace("{PRONOUN}", translation.Pronoun)
                .Replace("{NO_REVIEWS_SUMMARY}", translation.NoReviewsSummary)
                .Replace("{NO_REVIEWS_DEFAULT}", translation.NoReviewsDefault);

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
                var fallback = FallbackSummary(language);
                return (fallback, fallback);
            }
        }

        public async Task<ReviewStats> SummarizeIndeedAsync(
            string companyName,
            string indeedUrl,
            double? overallRating,
            string reviewsCount,
            double? workLife,
            double? payBenefits,
            double? jobSecurity,
            double? management,
            double? culture,
            List<string> reviewTitles,
            string language = "vi")
        {
            var titlesBlob = reviewTitles.Count > 0 
                ? string.Join("\n", reviewTitles.Select(t => $"- {t}")) 
                : "NO_REVIEW_TITLES";

            var translation = GetTranslation(language);
            var prompt = CompanyReviewsPrompt.IndeedSummarizationPrompt
                .Replace("{COMPANY_NAME}", companyName)
                .Replace("{OVERALL_RATING}", overallRating?.ToString("0.0") ?? "N/A")
                .Replace("{REVIEWS_COUNT}", string.IsNullOrWhiteSpace(reviewsCount) ? "N/A" : reviewsCount)
                .Replace("{DETAIL_WORK_LIFE}", workLife?.ToString("0.0") ?? "N/A")
                .Replace("{DETAIL_PAY_BENEFITS}", payBenefits?.ToString("0.0") ?? "N/A")
                .Replace("{DETAIL_SECURITY}", jobSecurity?.ToString("0.0") ?? "N/A")
                .Replace("{DETAIL_MANAGEMENT}", management?.ToString("0.0") ?? "N/A")
                .Replace("{DETAIL_CULTURE}", culture?.ToString("0.0") ?? "N/A")
                .Replace("{REVIEW_TITLES}", titlesBlob)
                .Replace("{LANGUAGE}", translation.LanguageName)
                .Replace("{PRONOUN}", translation.Pronoun);

            try
            {
                var responseText = await _llmClient.GetResponseAsync(new[] { new LlmChatMessage("user", prompt) }, 0);
                responseText = LlmTextHelper.CleanJsonResponse(responseText);

                using var doc = JsonDocument.Parse(responseText);
                var summary = ParseSummary(doc.RootElement);
                return summary;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Indeed AI review summarization: {ex.Message}");
                return FallbackSummary(language);
            }
        }

        public async Task<ReviewStats> SummarizeGlassdoorAsync(
            string companyName,
            string markdown,
            string language = "vi")
        {
            var translation = GetTranslation(language);
            var prompt = CompanyReviewsPrompt.GlassdoorSummarizationPrompt
                .Replace("{MARKDOWN}", markdown)
                .Replace("{LANGUAGE}", translation.LanguageName)
                .Replace("{PRONOUN}", translation.Pronoun);

            try
            {
                var responseText = await _llmClient.GetResponseAsync(new[] { new LlmChatMessage("user", prompt) }, 0);
                responseText = LlmTextHelper.CleanJsonResponse(responseText);

                using var doc = JsonDocument.Parse(responseText);
                var root = doc.RootElement;

                var stats = ParseSummary(root);

                if (root.TryGetProperty("overallRating", out var orProp) && (orProp.ValueKind == JsonValueKind.Number || orProp.ValueKind == JsonValueKind.String))
                {
                    stats.Rating = double.TryParse(orProp.ToString(), out var r) ? r : (double?)null;
                }
                
                if (root.TryGetProperty("reviewsCount", out var rcProp))
                {
                    stats.ReviewsCount = rcProp.ValueKind == JsonValueKind.String ? rcProp.GetString() ?? "" : rcProp.ToString();
                }

                if (root.TryGetProperty("diversityAndInclusion", out var diProp) && (diProp.ValueKind == JsonValueKind.Number || diProp.ValueKind == JsonValueKind.String))
                {
                    stats.DiversityAndInclusion = double.TryParse(diProp.ToString(), out var di) ? di : (double?)null;
                }

                var demographics = new List<DemographicRating>();
                if (root.TryGetProperty("demographicRatings", out var demoProp) && demoProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in demoProp.EnumerateArray())
                    {
                        var group = item.TryGetProperty("group", out var gProp) ? gProp.GetString() ?? "" : "";
                        double? rating = null;
                        if (item.TryGetProperty("rating", out var rProp) && (rProp.ValueKind == JsonValueKind.Number || rProp.ValueKind == JsonValueKind.String))
                        {
                            rating = double.TryParse(rProp.ToString(), out var rat) ? rat : (double?)null;
                        }
                        if (!string.IsNullOrEmpty(group))
                        {
                            demographics.Add(new DemographicRating { Group = group, Rating = rating });
                        }
                    }
                }
                stats.DemographicRatings = demographics;

                return stats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Glassdoor AI review summarization: {ex.Message}");
                return FallbackSummary(language);
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

            double? wl = el.TryGetProperty("workLifeBalance", out var wlProp) && (wlProp.ValueKind == JsonValueKind.Number || wlProp.ValueKind == JsonValueKind.String)
                ? (double.TryParse(wlProp.ToString(), out var parsedWl) ? parsedWl : (double?)null)
                : (double?)null;
            double? pb = el.TryGetProperty("payAndBenefits", out var pbProp) && (pbProp.ValueKind == JsonValueKind.Number || pbProp.ValueKind == JsonValueKind.String)
                ? (double.TryParse(pbProp.ToString(), out var parsedPb) ? parsedPb : (double?)null)
                : (double?)null;
            double? js = el.TryGetProperty("jobSecurityAndAdvancement", out var jsProp) && (jsProp.ValueKind == JsonValueKind.Number || jsProp.ValueKind == JsonValueKind.String)
                ? (double.TryParse(jsProp.ToString(), out var parsedJs) ? parsedJs : (double?)null)
                : (double?)null;
            double? mg = el.TryGetProperty("management", out var mgProp) && (mgProp.ValueKind == JsonValueKind.Number || mgProp.ValueKind == JsonValueKind.String)
                ? (double.TryParse(mgProp.ToString(), out var parsedMg) ? parsedMg : (double?)null)
                : (double?)null;
            double? cl = el.TryGetProperty("culture", out var clProp) && (clProp.ValueKind == JsonValueKind.Number || clProp.ValueKind == JsonValueKind.String)
                ? (double.TryParse(clProp.ToString(), out var parsedCl) ? parsedCl : (double?)null)
                : (double?)null;

            var titles = new List<string>();
            if (el.TryGetProperty("reviewTitles", out var titlesProp) && titlesProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var x in titlesProp.EnumerateArray())
                    titles.Add(x.GetString() ?? "");
            }

            return new ReviewStats
            {
                Summary = summary,
                Pros = pros,
                Cons = cons,
                Recommendations = recs,
                WorkLifeBalance = wl,
                PayAndBenefits = pb,
                JobSecurityAndAdvancement = js,
                Management = mg,
                Culture = cl,
                ReviewTitles = titles
            };
        }

        private static ReviewStats FallbackSummary(string language = "vi")
        {
            var translation = GetTranslation(language);
            return new ReviewStats
            {
                Summary = translation.FallbackSummary,
                Pros = new List<string> { translation.FallbackError },
                Cons = new List<string> { translation.FallbackError },
                Recommendations = new List<string> { translation.FallbackRecs }
            };
        }

        private struct ReviewTranslation
        {
            public string LanguageName { get; set; }
            public string Pronoun { get; set; }
            public string NoReviewsSummary { get; set; }
            public string NoReviewsDefault { get; set; }
            public string FallbackSummary { get; set; }
            public string FallbackError { get; set; }
            public string FallbackRecs { get; set; }
        }

        private static ReviewTranslation GetTranslation(string language)
        {
            if (language?.ToLower() == "en")
            {
                return new ReviewTranslation
                {
                    LanguageName = "English",
                    Pronoun = "you",
                    NoReviewsSummary = "No specific reviews have been recorded for this company yet.",
                    NoReviewsDefault = "No records",
                    FallbackSummary = "An error occurred while synthesizing reviews using AI.",
                    FallbackError = "Synthesis error",
                    FallbackRecs = "Please see the detailed sample reviews below"
                };
            }

            return new ReviewTranslation
            {
                LanguageName = "Vietnamese",
                Pronoun = "bạn",
                NoReviewsSummary = "Chưa có đánh giá cụ thể nào được ghi nhận cho công ty này.",
                NoReviewsDefault = "Chưa có ghi nhận",
                FallbackSummary = "Đã xảy ra lỗi khi tổng hợp nhận xét từ AI.",
                FallbackError = "Lỗi tổng hợp",
                FallbackRecs = "Vui lòng xem chi tiết ở các đánh giá ví dụ bên dưới"
            };
        }
    }
}
