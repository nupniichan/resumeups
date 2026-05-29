using System.Threading.Tasks;

namespace resumeups.Server.Interfaces
{
    public interface IGlassdoorSearchService
    {
        Task<(string? slug, string? host, string? html)> SearchGlassdoorSlugHostAndHtmlAsync(string companyName);
    }
}
