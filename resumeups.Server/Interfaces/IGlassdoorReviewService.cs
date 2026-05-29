using System.Threading.Tasks;
using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface IGlassdoorReviewService
    {
        Task<ReviewFetchResult> GetGlassdoorReviewsAsync(string companyName, string language = "vi");
    }
}
