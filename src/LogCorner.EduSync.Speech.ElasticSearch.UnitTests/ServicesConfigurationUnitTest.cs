using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace LogCorner.EduSync.Speech.ElasticSearch.UnitTests
{
    public class ServicesConfigurationUnitTest
    {
        [Fact]
        public void ShouldAddElasticSearch()
        {
            //Arrange
            IServiceCollection services = new ServiceCollection();

            //Act
            services.AddElasticSearch<ElasticStub>("http://localhost:9200", "testindex");
            var contains = services.ToList();
            var elasticSearchClient = contains.SingleOrDefault(c => c.Lifetime == ServiceLifetime.Singleton);

            //Assert
            Assert.NotNull(elasticSearchClient);
        }
    }
}