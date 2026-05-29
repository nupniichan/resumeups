using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;
using resumeups.Server.Services.Scrapers;

namespace resumeups.Server.Services
{
    public sealed class IndeedReviewService : IIndeedReviewService
    {
        private readonly IIndeedSearchService _searchService;
        private readonly IFirecrawlScraperService _scraperService;

        public IndeedReviewService(
            IIndeedSearchService searchService,
            IFirecrawlScraperService scraperService)
        {
            _searchService = searchService;
            _scraperService = scraperService;
        }

        public async Task<ReviewFetchResult> GetIndeedReviewsAsync(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
            {
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
            }

            try
            {
                var (slug, host, searchHtml) = await _searchService.SearchIndeedSlugHostAndHtmlAsync(companyName);
                if (string.IsNullOrEmpty(slug))
                {
                    Console.WriteLine($"Indeed: No company reviews URL found for '{companyName}'.");
                    return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
                }

                var resolvedHost = string.IsNullOrEmpty(host) ? "www.indeed.com" : host;
                var targetUrl = $"https://{resolvedHost}/cmp/{slug}/reviews";
                var scrapeUrl = $"{targetUrl}?fcountry=ALL";
                Console.WriteLine($"Indeed: Resolved company slug '{slug}' on host '{resolvedHost}'. Scraping URL: {scrapeUrl}");

                var firecrawlApiKey = EnvReader.Get("FIRECRAWL_API_KEY");
                if (string.IsNullOrWhiteSpace(firecrawlApiKey))
                {
                    Console.WriteLine("Indeed: FIRECRAWL_API_KEY is missing or empty.");
                    return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
                }

                var markdown = await _scraperService.ScrapeUrlWithFirecrawlAsync(scrapeUrl, firecrawlApiKey);
                if (string.IsNullOrWhiteSpace(markdown))
                {
                    Console.WriteLine("Indeed: Firecrawl returned empty response.");
                }

                var isBlocked = string.IsNullOrWhiteSpace(markdown) || 
                                (markdown.Contains("Base64-Image-Removed") && markdown.Length < 300);

                double? overallRating = null;
                string reviewsCount = "";
                double? workLife = null;
                double? payBenefits = null;
                double? jobSecurity = null;
                double? management = null;
                double? culture = null;
                var reviewTitles = new List<string>();

                if (isBlocked)
                {
                    Console.WriteLine("Indeed: Firecrawl scrape was blocked or returned empty. Applying Search Snippet fallback...");
                    var (fallbackRating, fallbackCount) = IndeedReviewParser.ExtractRatingsFromSearchHtml(searchHtml ?? "");
                    if (fallbackRating.HasValue)
                    {
                        overallRating = fallbackRating;
                        reviewsCount = fallbackCount;
                        Console.WriteLine($"Indeed: Recovered overall rating '{overallRating}' and count '{reviewsCount}' from search snippets.");
                    }
                    else
                    {
                        Console.WriteLine("Indeed: No ratings could be recovered dynamically from search snippets.");
                    }
                }
                else
                {
                    var parsed = IndeedReviewParser.ParseOverallRatingAndCount(markdown);
                    overallRating = parsed.overallRating;
                    reviewsCount = parsed.reviewsCount;
                    workLife = IndeedReviewParser.ParseCategoryScore(markdown, new[] { "work-life balance", "work/life balance", "cân bằng" });
                    payBenefits = IndeedReviewParser.ParseCategoryScore(markdown, new[] { "pay & benefits", "pay/benefits", "pay and benefits", "compensation", "lương" });
                    jobSecurity = IndeedReviewParser.ParseCategoryScore(markdown, new[] { "job security and advancement", "job security", "advancement", "career opportunities", "cơ hội", "ổn định" });
                    management = IndeedReviewParser.ParseCategoryScore(markdown, new[] { "management", "quản lý", "ban lãnh đạo" });
                    culture = IndeedReviewParser.ParseCategoryScore(markdown, new[] { "culture", "văn hóa", "môi trường" });
                    reviewTitles = IndeedReviewParser.ParseReviewTitles(markdown);
                }

                var partialStats = new ReviewStats
                {
                    Found = overallRating.HasValue,
                    Rating = overallRating,
                    LogoUrl = "", 
                    Website = "",
                    ReviewsUrl = targetUrl,
                    ReviewsCount = reviewsCount,
                    WorkLifeBalance = workLife,
                    PayAndBenefits = payBenefits,
                    JobSecurityAndAdvancement = jobSecurity,
                    Management = management,
                    Culture = culture,
                    ReviewTitles = reviewTitles
                };

                var rawReviewsText = reviewTitles.Select(t => $"Tiêu đề: {t}").ToList();

                return new ReviewFetchResult
                {
                    PartialStats = partialStats,
                    RawReviews = rawReviewsText
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Indeed: Error during reviews crawl: {ex.Message}");
                return new ReviewFetchResult { PartialStats = new ReviewStats { Found = false } };
            }
        }
    }
}
