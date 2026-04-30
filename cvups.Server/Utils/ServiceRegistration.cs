using cvups.Server.Interfaces;
using cvups.Server.Services;

namespace cvups.Server.Utils
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICVExtractor, PDFCVExtractorService>();
            services.AddScoped<ICVExtractor, WordCVExtractorService>();
            services.AddScoped<IPiiMasker, PiiMaskerService>();

            return services;
        }
    }
}