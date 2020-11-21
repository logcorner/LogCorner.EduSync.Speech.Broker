using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace LogCorner.EduSync.SignalR.Common.UnitTests
{
    public class ServicesConfigurationUnitTest
    {
        [Fact]
        public void ShouldAddSharedKernel()
        {
            //Arrange
            IServiceCollection services = new ServiceCollection();

            //Act
            services.AddSignalRServices("");
            var contains = services.ToList();
            var signalRNotifier = contains.SingleOrDefault(c => c.ServiceType.Name == nameof(ISignalRNotifier) && c.Lifetime == ServiceLifetime.Singleton);
            var signalRPublisher = contains.SingleOrDefault(c => c.ServiceType.Name == nameof(ISignalRPublisher) && c.Lifetime == ServiceLifetime.Singleton);
            var hubInstance = contains.SingleOrDefault(c => c.ServiceType.Name == nameof(IHubInstance) && c.Lifetime == ServiceLifetime.Singleton);

            //Assert
            Assert.Equal(signalRNotifier?.ImplementationType?.FullName, typeof(SignalRNotifier).FullName);
            Assert.Equal(signalRPublisher?.ImplementationType?.FullName, typeof(SignalRPublisher).FullName);
            Assert.NotNull(hubInstance);
        }
    }
}