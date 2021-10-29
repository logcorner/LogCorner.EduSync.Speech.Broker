using Elasticsearch.Net;
using LogCorner.EduSync.Speech.Projection;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ElasticSearch
{
    public class ElasticSearchClient<T> : IElasticSearchClient<T> where T : Entity<Guid>
    {
        private readonly string _indexName;

        private ElasticClient _client;

        public ElasticSearchClient(string indexName)
        {
            _indexName = indexName;
        }

        public void Init(string url)
        {
            var pool = new SingleNodeConnectionPool(new Uri(url));
            var connectionSettings =
                new ConnectionSettings(pool, (builtin, settings) => new JsonNetSerializer(
                    builtin, settings,
                    () => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore },
                    resolver => resolver.NamingStrategy = new SnakeCaseNamingStrategy()
                ))
                    .EnableDebugMode()
                    .PrettyJson();

            _client = new ElasticClient(connectionSettings);

            var result = _client.Indices.Exists(Indices.Index(_indexName));
            if (!result.Exists)
            {
                var createIndexResponse = _client.Indices.Create(_indexName, c => c
                    .Map<T>(m => m.AutoMap())
                );
                if (!createIndexResponse.IsValid)
                {
                    throw new Exception($"Cannot initialyze elastci search {url} {_indexName} {createIndexResponse.OriginalException}");
                }
            }
        }

        public async Task CreateAsync(T entity)
        {
            var result = await _client.UpdateAsync<T>(entity.Id,
                u =>
                    u.Index(_indexName)
                        .Doc(entity)
                        .DocAsUpsert()
                        .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new Exception("Error occured during update", result.OriginalException);

            Console.WriteLine($"**ElasticSearchClient::CreateAsync - indexName = {_indexName}");
            Console.WriteLine($"**ElasticSearchClient::CreateAsync - entity = {entity.Id}");
        }

        public async Task DeleteAsync(T entity)
        {
            var result = await _client.DeleteAsync<T>(entity.Id, u => u.Index(_indexName).Refresh(Refresh.True));
            if (!result.IsValid)
                throw new Exception("Error occured during delete", result.OriginalException);
            Console.WriteLine($"**ElasticSearchClient::DeleteAsync - entity = {entity.Id}");
        }
    }
}