using System.Threading.Tasks;
using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface IReviewCongTyService
    {
        Task<ReviewCongTyCandidateData> SearchCandidatesAsync(string companyName);
        Task<ReviewFetchResult> FetchReviewsAsync(string slug);
    }
}
