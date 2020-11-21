using LogCorner.EduSync.Speech.Projection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.ElasticSearch.UnitTests
{
    //TODO : refactor  this
    public class ElasticSearchClientUnitTest
    {
        [Fact]
        public void ShouldInitElasticSearchClient()
        {
            var url = "http://localhost:1212";
            var indexName = "myindex";
            var client = new ElasticSearchClient<Entity<Guid>>(indexName);
            var result = client.Init(url);

            Assert.NotNull(result.OriginalException);
        }

        [Fact]
        public async Task ShouldCreateIndex()
        {
            var url = "http://localhost:1212";
            var indexName = "myindex";
            var client = new ElasticSearchClient<Entity<Guid>>(indexName);

            client.Init(url);
            await client.CreateAsync(new ElasticStub());
        }

        [Fact]
        public async Task ShouldDeleteIndex()
        {
            var url = "http://localhost:1212";
            var indexName = "myindex";
            var client = new ElasticSearchClient<Entity<Guid>>(indexName);

            client.Init(url);
            await client.DeleteAsync(new ElasticStub());
        }
    }
}