using Elasticsearch.Net;
using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
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

        public AcknowledgedResponseBase Init(string url)
        {
            var pool = new SingleNodeConnectionPool(new Uri(url));
            var connectionSettings =
                new ConnectionSettings(pool, sourceSerializer: (builtin, settings) => new JsonNetSerializer(
                    builtin, settings,
                    () => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore },
                    resolver => resolver.NamingStrategy = new SnakeCaseNamingStrategy()
                ))
                    .EnableDebugMode()
                    .PrettyJson();

            _client = new ElasticClient(connectionSettings);

            var result = _client.Indices.Exists(Indices.Index(_indexName));
            if (result.Exists) return new CreateIndexResponse();
            var createIndexResponse = _client.Indices.Create(_indexName, c => c
                .Map<T>(m => m.AutoMap())
            );
            return createIndexResponse;
        }

        public async Task CreateAsync(T entity)
        {
            var result = await _client.UpdateAsync<T>(entity.Id,
                u =>
                    u.Index(_indexName)
                        .Doc(entity)
                        .DocAsUpsert());

            if (!result.IsValid)
                throw new Exception("Error occured during update for ElasticReminderEntry", result.OriginalException);
        }
    }
}