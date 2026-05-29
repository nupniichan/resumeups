using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using resumeups.Server.Interfaces;

namespace resumeups.Server.Services.Scrapers
{
    public sealed class FirecrawlScraperService : IFirecrawlScraperService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FirecrawlScraperService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> ScrapeUrlWithFirecrawlAsync(string url, string apiKey)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                url = url,
                formats = new[] { "markdown" },
                onlyMainContent = false,
                waitFor = 1000
            };

            using var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("https://api.firecrawl.dev/v1/scrape", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Indeed Scraper: Firecrawl API scrape call failed with status: {response.StatusCode}");
                return string.Empty;
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            if (doc.RootElement.TryGetProperty("success", out var successProp) && successProp.GetBoolean())
            {
                var data = doc.RootElement.GetProperty("data");
                return data.GetProperty("markdown").GetString() ?? string.Empty;
            }

            return string.Empty;
        }

        public async Task<string> SearchWithFirecrawlAsync(string query, string apiKey, int limit = 5)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                query = query,
                limit = limit
            };

            using var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("https://api.firecrawl.dev/v1/search", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Firecrawl Search: API call failed with status: {response.StatusCode}");
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
