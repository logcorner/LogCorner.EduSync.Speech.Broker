using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.Speech.Resiliency
{
    public static class ServicesConfiguration
    {
        public static void AddResiliencyServices(this IServiceCollection services)
        {
            services.AddSingleton<IResiliencyService, ResiliencyService>();
        }
    }
}