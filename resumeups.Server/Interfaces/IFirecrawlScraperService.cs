using System.Threading.Tasks;

namespace resumeups.Server.Interfaces
{
    public interface IFirecrawlScraperService
    {
        Task<string> ScrapeUrlWithFirecrawlAsync(string url, string apiKey);
    }
}
