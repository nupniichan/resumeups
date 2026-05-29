using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;
using resumeups.Server.Services.Scrapers;

namespace resumeups.Server.Services
{
    public sealed class GlassdoorReviewService : IGlassdoorReviewService
    {
        private readonly IGlassdoorSearchService _searchService;
        private readonly IFirecrawlScraperService _scraperService;
        private readonly IReviewSummarizerService _summarizer;

        public GlassdoorReviewService(
            IGlassdoorSearchService searchService,
            IFirecrawlScraperService scraperService,
            IReviewSummarizerService summarizer)
        {
            _searchService = searchService;
            _scraperService = scraperService;
            _summarizer = summarizer;
        }

        public async Task<ReviewFetchResult> GetGlassdoorReviewsAsync(string companyName, string language = "vi")
        {
            if (string.IsNullOrWhiteSpace(companyName))
            {
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
            }

            try
            {
                var (slug, host, searchHtml) = await _searchService.SearchGlassdoorSlugHostAndHtmlAsync(companyName);
                if (string.IsNullOrEmpty(slug))
                {
                    Console.WriteLine($"Glassdoor: No company reviews URL found for '{companyName}'.");
                    return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
                }

                var resolvedHost = string.IsNullOrEmpty(host) ? "www.glassdoor.com" : host;
                var targetUrl = $"https://{resolvedHost}/Reviews/{slug}";
                Console.WriteLine($"Glassdoor: Resolved company slug '{slug}' on host '{resolvedHost}'. Scraping URL: {targetUrl}");

                var firecrawlApiKey = EnvReader.Get("FIRECRAWL_API_KEY");
                if (string.IsNullOrWhiteSpace(firecrawlApiKey))
                {
                    Console.WriteLine("Glassdoor: FIRECRAWL_API_KEY is missing or empty.");
                    return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
                }

                var rawMarkdown = await _scraperService.ScrapeUrlWithFirecrawlAsync(targetUrl, firecrawlApiKey);
                if (string.IsNullOrWhiteSpace(rawMarkdown))
                {
                    Console.WriteLine("Glassdoor: Firecrawl returned empty response.");
                    return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
                }



                var cleanedMarkdown = CleanMarkdown(rawMarkdown);
                var stats = await _summarizer.SummarizeGlassdoorAsync(companyName, cleanedMarkdown, language);
                
                stats.Found = stats.Rating.HasValue || !string.IsNullOrEmpty(stats.Summary);
                stats.Website = "";
                stats.ReviewsUrl = targetUrl;
                stats.LogoUrl = "";

                return new ReviewFetchResult
                {
                    PartialStats = stats,
                    RawReviews = stats.ReviewTitles.Select(t => $"Review: {t}").ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Glassdoor: Error during reviews crawl: {ex.Message}");
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
            }
        }

        private static string CleanMarkdown(string markdown)
        {
            if (string.IsNullOrEmpty(markdown)) return string.Empty;
            
            var lines = markdown.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var filtered = lines
                .Select(l => l.Trim())
                .Where(l => !l.Contains("data:image/") && !l.Contains("base64") && !l.Contains("Base64-Image-Removed"))
                .ToList();
                
            return string.Join("\n", filtered);
        }
    }
}
