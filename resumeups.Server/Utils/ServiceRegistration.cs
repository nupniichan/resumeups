using resumeups.Server.Interfaces;
using resumeups.Server.Services;

namespace resumeups.Server.Utils
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IResumeExtractor, PdfExtractorService>();
            services.AddScoped<IResumeExtractor, WordExtractorService>();
            services.AddScoped<IPiiMasker, PiiMaskerService>();

            return services;
        }
    }
}