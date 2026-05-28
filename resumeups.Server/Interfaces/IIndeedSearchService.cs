using System.Threading.Tasks;

namespace resumeups.Server.Interfaces
{
    public interface IIndeedSearchService
    {
        Task<(string? slug, string? host, string? html)> SearchIndeedSlugHostAndHtmlAsync(string companyName);
    }
}
