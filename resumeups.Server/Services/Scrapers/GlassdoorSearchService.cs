using System;
using System.Net.Http;
using System.Threading.Tasks;
using resumeups.Server.Interfaces;
using resumeups.Server.Utils;

namespace resumeups.Server.Services.Scrapers
{
    public sealed class GlassdoorSearchService : IGlassdoorSearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFirecrawlScraperService _scraperService;

        public GlassdoorSearchService(
            IHttpClientFactory httpClientFactory,
            IFirecrawlScraperService scraperService)
        {
            _httpClientFactory = httpClientFactory;
            _scraperService = scraperService;
        }

        public async Task<(string? slug, string? host, string? html)> SearchGlassdoorSlugHostAndHtmlAsync(string companyName)
        {
            var query = $"{companyName} glassdoor company review";

            var firecrawlApiKey = EnvReader.Get("FIRECRAWL_API_KEY");
            if (!string.IsNullOrWhiteSpace(firecrawlApiKey))
            {
                try
                {
                    var searchJson = await _scraperService.SearchWithFirecrawlAsync(query, firecrawlApiKey);
                    if (!string.IsNullOrWhiteSpace(searchJson))
                    {
                        using var doc = System.Text.Json.JsonDocument.Parse(searchJson);
                        if (doc.RootElement.TryGetProperty("success", out var successProp) && successProp.GetBoolean())
                        {
                            var data = doc.RootElement.GetProperty("data");
                            foreach (var item in data.EnumerateArray())
                            {
                                var url = item.TryGetProperty("url", out var uProp) ? uProp.GetString() : null;
                                if (!string.IsNullOrEmpty(url))
                                {
                                    var (slug, host) = GlassdoorReviewParser.ExtractGlassdoorSlugAndHost(url);
                                    if (!string.IsNullOrEmpty(slug))
                                    {
                                        return (slug, host, searchJson);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Glassdoor Search: Firecrawl Search exception: {ex.Message}. Trying scrapers fallback...");
                }
            }

            var client = CreateHttpClient();
            var searchUrl = $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";

            string? searchHtml = null;
            try
            {
                var response = await client.GetAsync(searchUrl);
                if (response.IsSuccessStatusCode)
                {
                    searchHtml = await response.Content.ReadAsStringAsync();
                    var (slug, host) = GlassdoorReviewParser.ExtractGlassdoorSlugAndHost(searchHtml);
                    if (!string.IsNullOrEmpty(slug))
                    {
                        return (slug, host, searchHtml);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Glassdoor Search: Google Search exception: {ex.Message}");
            }

            try
            {
                var ddgUrl = $"https://html.duckduckgo.com/html/?q={Uri.EscapeDataString(query)}";
                var response = await client.GetAsync(ddgUrl);
                if (response.IsSuccessStatusCode)
                {
                    searchHtml = await response.Content.ReadAsStringAsync();
                    var (slug, host) = GlassdoorReviewParser.ExtractGlassdoorSlugAndHost(searchHtml);
                    return (slug, host, searchHtml);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Glassdoor Search: DuckDuckGo search fallback failed: {ex.Message}");
            }
            return (null, null, searchHtml);
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Language", "vi,en-US;q=0.9,en;q=0.8");
            return client;
        }
    }
}
