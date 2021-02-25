using LogCorner.EduSync.Speech.Projection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LogCorner.EduSync.Speech.ElasticSearch
{
    public static class ServicesConfiguration
    {
        public static void AddElasticSearch<T>(this IServiceCollection services, string url, string index) where T : Entity<Guid>
        {
            services.AddSingleton<IElasticSearchClient<T>, ElasticSearchClient<T>>(ctx =>
            {
                var elasticSearchClient = new ElasticSearchClient<T>(index);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    elasticSearchClient.Init(url);
                }

                return elasticSearchClient;
            });
        }
    }
}