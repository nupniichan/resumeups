using System.Threading.Tasks;
using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface INote8ReviewService
    {
        Task<Note8CandidateData> SearchCandidatesAsync(string companyName);
        Task<ReviewFetchResult> FetchReviewsAsync(string slug, Note8CandidateData candidateData);
    }
}
