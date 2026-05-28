using System.Threading.Tasks;
using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface IIndeedReviewService
    {
        Task<ReviewFetchResult> GetIndeedReviewsAsync(string companyName);
    }
}
