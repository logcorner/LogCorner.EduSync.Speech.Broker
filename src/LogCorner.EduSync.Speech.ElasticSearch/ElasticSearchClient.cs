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
        // private readonly ILogger<ElasticSearchClient<T>> _logger;

        private ElasticClient _client;

        public ElasticSearchClient(string indexName/*, ILogger<ElasticSearchClient<T>> logger*/)
        {
            _indexName = indexName;
            //_logger = logger;
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
            ExistsResponse result;
            try
            {
                result = _client.Indices.Exists(Indices.Index(_indexName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            if (!result.Exists)
            {
                var createIndexResponse = _client.Indices.Create(_indexName, c => c
                    .Map<T>(m => m.AutoMap())
                );
                if (!createIndexResponse.IsValid)
                {
                    throw new ElasticSearchException($"Cannot initialyze elastci search {url} {_indexName} {createIndexResponse.OriginalException}");
                }
            }
        }

        private async Task CreateAsync(T entity)
        {
            var result = await _client.UpdateAsync<T>(entity.Id,
                u =>
                    u.Index(_indexName)
                        .Doc(entity)
                        .DocAsUpsert()
                        .Refresh(Refresh.True));

            if (!result.IsValid)
                throw new ElasticSearchException($"Error occured during update {result.OriginalException}");

            Console.WriteLine($"**ElasticSearchClient::CreateAsync - indexName = {_indexName}");
            Console.WriteLine($"**ElasticSearchClient::CreateAsync - entity = {entity.Id}");
        }

        private async Task DeleteAsync(T entity)
        {
            var result = await _client.DeleteAsync<T>(entity.Id, u => u.Index(_indexName).Refresh(Refresh.True));
            if (!result.IsValid)
                throw new ElasticSearchException($"Error occured during delete {result.OriginalException}");
            Console.WriteLine($"**ElasticSearchClient::DeleteAsync - entity = {entity.Id}");
        }

        public async Task ProjectEventAsync(T entity, bool isDeleted = false)
        {
            if (isDeleted)
            {
                await DeleteAsync(entity);
            }
            else
            {
                await CreateAsync(entity);
            }
        }
    }
}