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
    public sealed class ReviewCongTyService : IReviewCongTyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReviewCongTyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ReviewCongTyCandidateData> SearchCandidatesAsync(string companyName)
        {
            var client = CreateHttpClient();
            var searchUrl = $"https://reviewcongty.vn/danh-sach-cong-ty?crp_s={Uri.EscapeDataString(companyName)}";
            var response = await client.GetAsync(searchUrl);
            if (!response.IsSuccessStatusCode)
                return new ReviewCongTyCandidateData { HasResults = false };

            var htmlContent = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            var companyNodes = htmlDoc.DocumentNode.SelectNodes("//a[contains(@class, 'crp-row-title')]");
            if (companyNodes == null || companyNodes.Count == 0)
                return new ReviewCongTyCandidateData { HasResults = false };

            var candidates = new List<object>();
            foreach (var node in companyNodes)
            {
                var text = node.InnerText?.Trim() ?? "";
                var href = node.GetAttributeValue("href", "") ?? "";
                var match = Regex.Match(href, @"/cong-ty/([^/]+)");
                if (match.Success)
                    candidates.Add(new { text, slug = match.Groups[1].Value });
            }

            if (candidates.Count == 0)
                return new ReviewCongTyCandidateData { HasResults = false };

            return new ReviewCongTyCandidateData
            {
                HasResults = true,
                CandidatesJson = JsonSerializer.Serialize(candidates)
            };
        }

        public async Task<ReviewFetchResult> FetchReviewsAsync(string slug)
        {
            if (slug == "NOT_FOUND")
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };

            var client = CreateHttpClient();
            var detailsUrl = $"https://reviewcongty.vn/cong-ty/{slug}";
            var detailsResponse = await client.GetAsync(detailsUrl);
            if (!detailsResponse.IsSuccessStatusCode)
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };

            var detailsHtml = await detailsResponse.Content.ReadAsStringAsync();
            var detailsDoc = new HtmlDocument();
            detailsDoc.LoadHtml(detailsHtml);

            var ratingNumNode = detailsDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'crp-rating-num')]");
            double? ratingScore = double.TryParse(ratingNumNode?.InnerText?.Trim(), out var parsedScore) ? parsedScore : (double?)null;

            var globeNode = detailsDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'crp-company-meta-item')]//i[contains(@class, 'fa-globe')]/following-sibling::a") ??
                            detailsDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'crp-company-meta-item')]//a[contains(@href, 'http')]");
            string websiteUrl = globeNode?.GetAttributeValue("href", "") ?? "";

            var avatarNode = detailsDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'crp-company-header-avatar')]//img");
            string logoUrl = avatarNode?.GetAttributeValue("src", "") ?? "";

            var reviewNodes = detailsDoc.DocumentNode.SelectNodes("//div[contains(@class, 'crp-review-item')]");
            var rawReviewsText = new List<string>();

            if (reviewNodes != null)
            {
                foreach (var node in reviewNodes.Take(10))
                {
                    var prosNode = node.SelectSingleNode(".//div[contains(@class, 'crp-review-pros')]/p");
                    var consNode = node.SelectSingleNode(".//div[contains(@class, 'crp-review-cons')]/p");

                    string pros = prosNode?.InnerText?.Trim() ?? "";
                    string cons = consNode?.InnerText?.Trim() ?? "";

                    rawReviewsText.Add($"Ưu điểm: {pros}\nNhược điểm: {cons}");
                }
            }

            var reviewsCount = "";
            var countNode = detailsDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'crp-rating-count')]") ??
                            detailsDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'crp-review-count')]") ??
                            detailsDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'crp-company-reviews-count')]") ??
                            detailsDoc.DocumentNode.SelectSingleNode("//span[contains(text(), 'đánh giá')]");
            if (countNode != null)
            {
                var match = Regex.Match(countNode.InnerText, @"\d+");
                if (match.Success) reviewsCount = match.Value;
            }

            if (string.IsNullOrEmpty(reviewsCount))
            {
                var match = Regex.Match(detailsHtml, @"\b(\d+)\s*(?:đánh giá|nhận xét|reviews)\b", RegexOptions.IgnoreCase);
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
                    Rating = ratingScore,
                    LogoUrl = logoUrl,
                    Website = websiteUrl,
                    ReviewsUrl = detailsUrl,
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
