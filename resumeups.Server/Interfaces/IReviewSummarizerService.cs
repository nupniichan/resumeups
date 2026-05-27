using System.Collections.Generic;
using System.Threading.Tasks;
using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface IReviewSummarizerService
    {
        Task<MultiSiteMatchResult> MatchCompaniesAsync(
            string companyName,
            Note8CandidateData note8Candidates,
            ReviewCongTyCandidateData rctCandidates);

        Task<(ReviewStats note8, ReviewStats reviewCongTy)> SummarizeBothAsync(
            List<string> note8Reviews,
            List<string> rctReviews);
    }
}
