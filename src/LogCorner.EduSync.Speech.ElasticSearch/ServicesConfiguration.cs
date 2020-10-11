using Microsoft.Extensions.DependencyInjection;
using System;
using LogCorner.EduSync.Speech.Projection;

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
                    var setup = elasticSearchClient.Init(url);

                    if (setup.IsValid)
                    {
                        throw new Exception($"Cannot initialyze elastci search {url} {index} {setup.OriginalException}");
                    }
                }

                return elasticSearchClient;
            });
        }
    }
}