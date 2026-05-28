using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;

namespace resumeups.Server.Services
{
    public sealed class Note8ReviewService : INote8ReviewService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Note8ReviewService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc/>
        public async Task<Note8CandidateData> SearchCandidatesAsync(string companyName)
        {
            var client = CreateHttpClient();
            var searchUrl = $"https://note8.vn/job-suggest/companies?q={Uri.EscapeDataString(companyName)}";
            var response = await client.GetAsync(searchUrl);
            if (!response.IsSuccessStatusCode)
                return new Note8CandidateData { HasResults = false };

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var resultsArray = doc.RootElement.GetProperty("results");

            if (resultsArray.GetArrayLength() == 0)
                return new Note8CandidateData { HasResults = false };

            var candidates = new List<object>();
            var metaBySlug = new Dictionary<string, Note8CompanyMeta>();

            foreach (var item in resultsArray.EnumerateArray())
            {
                var id = item.GetProperty("id").GetInt32();
                var slug = item.GetProperty("slug").GetString() ?? "";
                var text = item.GetProperty("text").GetString() ?? "";
                var website = item.TryGetProperty("website", out var web) ? web.GetString() : "";
                var averageRating = item.TryGetProperty("average_rating", out var rate) && rate.ValueKind == JsonValueKind.Number
                    ? rate.GetDouble()
                    : (double?)null;
                var logoPath = item.TryGetProperty("logo", out var logo) ? (logo.GetString() ?? "") : "";
                var logoUrl = !string.IsNullOrEmpty(logoPath) ? $"https://note8.vn/storage/{logoPath}" : "";

                candidates.Add(new { text, id, slug, website });
                metaBySlug[slug] = new Note8CompanyMeta
                {
                    Id = id,
                    AverageRating = averageRating,
                    LogoUrl = logoUrl,
                    Website = website ?? ""
                };
            }

            return new Note8CandidateData
            {
                HasResults = true,
                CandidatesJson = JsonSerializer.Serialize(candidates),
                MetaBySlug = metaBySlug
            };
        }

        /// <inheritdoc/>
        public async Task<ReviewFetchResult> FetchReviewsAsync(string slug, Note8CandidateData candidateData)
        {
            if (slug == "NOT_FOUND" || !candidateData.MetaBySlug.TryGetValue(slug, out var meta))
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };

            var client = CreateHttpClient();
            var reviewsUrl = $"https://note8.vn/danh-gia-dn/{slug}-{meta.Id}";
            var reviewsResponse = await client.GetAsync(reviewsUrl);
            if (!reviewsResponse.IsSuccessStatusCode)
            {
                return new ReviewFetchResult
                {
                    PartialStats = new ReviewStats
                    {
                        Found = true,
                        Rating = meta.AverageRating,
                        LogoUrl = meta.LogoUrl,
                        Website = meta.Website
                    }
                };
            }

            var htmlContent = await reviewsResponse.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            var reviewNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'ReviewItem')]");
            var rawReviewsText = new List<string>();

            if (reviewNodes != null)
            {
                foreach (var node in reviewNodes.Take(10))
                {
                    var titleNode = node.SelectSingleNode(".//h2[contains(@class, 'ReviewTitle')]");
                    var ratingNode = node.SelectSingleNode(".//span[contains(@class, 'ratingNumber')]");
                    var prosNode = node.SelectSingleNode(".//strong[contains(@class, 'greenColor')]/following-sibling::div[1]");
                    var consNode = node.SelectSingleNode(".//strong[contains(@class, 'text-danger')]/following-sibling::div[1]");

                    string title = titleNode?.InnerText?.Trim() ?? "";
                    double? rating = double.TryParse(ratingNode?.InnerText?.Trim(), out var parsedRating) ? parsedRating : (double?)null;
                    string pros = prosNode?.InnerText?.Trim() ?? "";
                    string cons = consNode?.InnerText?.Trim() ?? "";

                    rawReviewsText.Add($"Tiêu đề: {title}\nĐánh giá: {rating} sao\nƯu điểm: {pros}\nNhược điểm: {cons}");
                }
            }

            var reviewsCount = "";
            var countNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'ReviewCount')]") ??
                            htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'ReviewCount')]") ??
                            htmlDoc.DocumentNode.SelectSingleNode("//a[contains(@href, '#reviews')]");
            if (countNode != null)
            {
                var match = Regex.Match(countNode.InnerText, @"\d+");
                if (match.Success) reviewsCount = match.Value;
            }

            if (string.IsNullOrEmpty(reviewsCount))
            {
                var match = Regex.Match(htmlContent, @"\b(\d+)\s*(?:đánh giá|nhận xét|reviews)\b", RegexOptions.IgnoreCase);
                if (match.Success) reviewsCount = match.Groups[1].Value;
            }

            if (string.IsNullOrEmpty(reviewsCount) && reviewNodes != null)
            {
                reviewsCount = reviewNodes.Count.ToString();
            }

            return new ReviewFetchResult
            {
                PartialStats = new ReviewStats
                {
                    Found = true,
                    Rating = meta.AverageRating,
                    LogoUrl = meta.LogoUrl,
                    Website = meta.Website,
                    ReviewsCount = reviewsCount
                },
                RawReviews = rawReviewsText
            };
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Language", "vi,en-US;q=0.9,en;q=0.8");
            return client;
        }
    }
}
