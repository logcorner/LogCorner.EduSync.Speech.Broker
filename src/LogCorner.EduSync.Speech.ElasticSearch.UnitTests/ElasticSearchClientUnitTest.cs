using System;
using LogCorner.EduSync.Speech.Projection;
using Xunit;

namespace LogCorner.EduSync.Speech.ElasticSearch.UnitTests
{
    //TODO : refactor
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
        public void ShouldCreateIndex()
        {
            var url = "http://localhost:1212";
            var indexName = "myindex";
            var client = new ElasticSearchClient<Entity<Guid>>(indexName);

            client.Init(url);
            var result = client.CreateAsync(new ElasticStub());
        }
    }
}