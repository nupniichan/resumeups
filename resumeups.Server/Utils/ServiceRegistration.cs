using System.Net.Http.Headers;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Services;
using resumeups.Server.Services.Llm;
using resumeups.Server.Services.Scrapers;

namespace resumeups.Server.Utils
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton(LlmSettings.FromEnvironment());

            services.AddHttpClient("llm", static (sp, client) =>
            {
                var settings = sp.GetRequiredService<LlmSettings>();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", settings.ApiKey);
            });

            services.AddScoped<ILLMClient, OpenAiCompatible>();

            services.AddScoped<IResumeExtractorService, PdfExtractorService>();
            services.AddScoped<IResumeExtractorService, WordExtractorService>();
            services.AddScoped<IPiiMaskerService, PiiMaskerService>();
            services.AddScoped<IMatchingService, MatchingService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IAnalyzeService, AnalyzeService>();
            services.AddScoped<IReviewSummarizerService, ReviewSummarizerService>();
            services.AddScoped<INote8ReviewService, Note8ReviewService>();
            services.AddScoped<IReviewCongTyService, ReviewCongTyService>();
            services.AddScoped<IIndeedSearchService, IndeedSearchService>();
            services.AddScoped<IFirecrawlScraperService, FirecrawlScraperService>();
            services.AddScoped<IIndeedReviewService, IndeedReviewService>();
            services.AddScoped<IGlassdoorSearchService, GlassdoorSearchService>();
            services.AddScoped<IGlassdoorReviewService, GlassdoorReviewService>();
            services.AddScoped<ICompanyReviewsService, CompanyReviewsService>();

            return services;
        }
    }
}