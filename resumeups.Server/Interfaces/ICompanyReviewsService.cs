using System.Threading.Tasks;
using resumeups.Server.Models;

namespace resumeups.Server.Interfaces
{
    public interface ICompanyReviewsService
    {
        Task<CompanyReviewResult> GetCompanyReviewsAsync(string companyName, string language = "vi");
    }
}
