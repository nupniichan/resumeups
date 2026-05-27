using System;
using System.Threading.Tasks;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;

namespace resumeups.Server.Services
{
    public sealed class CompanyReviewsService : ICompanyReviewsService
    {
        private readonly INote8ReviewService _note8Service;
        private readonly IReviewCongTyService _rctService;
        private readonly IReviewSummarizerService _summarizer;

        public CompanyReviewsService(
            INote8ReviewService note8Service,
            IReviewCongTyService rctService,
            IReviewSummarizerService summarizer)
        {
            _note8Service = note8Service;
            _rctService = rctService;
            _summarizer = summarizer;
        }

        public async Task<CompanyReviewResult> GetCompanyReviewsAsync(string companyName)
        {
            var result = new CompanyReviewResult { CompanyName = companyName };

            var note8SearchTask = _note8Service.SearchCandidatesAsync(companyName);
            var rctSearchTask = _rctService.SearchCandidatesAsync(companyName);

            Note8CandidateData note8Candidates;
            ReviewCongTyCandidateData rctCandidates;
            try
            {
                await Task.WhenAll(note8SearchTask, rctSearchTask);
                note8Candidates = await note8SearchTask;
                rctCandidates = await rctSearchTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Phase 1 (search): {ex.Message}");
                result.Note8 = new ReviewStats { Found = false };
                result.ReviewCongTy = new ReviewStats { Found = false };
                return result;
            }

            MultiSiteMatchResult matchResult;
            try
            {
                matchResult = await _summarizer.MatchCompaniesAsync(companyName, note8Candidates, rctCandidates);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Phase 2 (LLM matching): {ex.Message}");
                result.Note8 = new ReviewStats { Found = false };
                result.ReviewCongTy = new ReviewStats { Found = false };
                return result;
            }

            ReviewFetchResult note8Fetch = new() { PartialStats = new ReviewStats { Found = false } };
            ReviewFetchResult rctFetch = new() { PartialStats = new ReviewStats { Found = false } };

            var note8FetchTask = SafeFetch(() => _note8Service.FetchReviewsAsync(matchResult.Note8Slug, note8Candidates), "Note8");
            var rctFetchTask = SafeFetch(() => _rctService.FetchReviewsAsync(matchResult.ReviewCongTySlug), "ReviewCongTy");

            await Task.WhenAll(note8FetchTask, rctFetchTask);
            note8Fetch = await note8FetchTask;
            rctFetch = await rctFetchTask;

            try
            {
                var (note8Summary, rctSummary) = await _summarizer.SummarizeBothAsync(
                    note8Fetch.RawReviews,
                    rctFetch.RawReviews);

                ApplySummary(note8Fetch.PartialStats, note8Summary);
                ApplySummary(rctFetch.PartialStats, rctSummary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Phase 4 (LLM summarization): {ex.Message}");
            }

            result.Note8 = note8Fetch.PartialStats;
            result.ReviewCongTy = rctFetch.PartialStats;
            return result;
        }

        private static async Task<ReviewFetchResult> SafeFetch(
            Func<Task<ReviewFetchResult>> fetch,
            string sourceName)
        {
            try
            {
                return await fetch();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Phase 3 fetch ({sourceName}): {ex.Message}");
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
            }
        }

        private static void ApplySummary(ReviewStats target, ReviewStats summary)
        {
            target.Summary = summary.Summary;
            target.Pros = summary.Pros;
            target.Cons = summary.Cons;
            target.Recommendations = summary.Recommendations;
        }
    }
}
