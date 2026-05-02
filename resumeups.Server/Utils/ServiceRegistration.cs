using resumeups.Server.Interfaces;
using resumeups.Server.Services;

namespace resumeups.Server.Utils
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IResumeExtractorService, PdfExtractorService>();
            services.AddScoped<IResumeExtractorService, WordExtractorService>();
            services.AddScoped<IPiiMaskerService, PiiMaskerService>();
            services.AddScoped<IMatchingService, MatchingService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IAnalyzeService, AnalyzeService>();

            return services;
        }
    }
}