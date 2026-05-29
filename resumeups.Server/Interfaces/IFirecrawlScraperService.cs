using System.Threading.Tasks;

namespace resumeups.Server.Interfaces
{
    public interface IFirecrawlScraperService
    {
        Task<string> ScrapeUrlWithFirecrawlAsync(string url, string apiKey);
        Task<string> SearchWithFirecrawlAsync(string query, string apiKey, int limit = 5);
    }
}
