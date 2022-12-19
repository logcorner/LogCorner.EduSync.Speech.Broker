//using LogCorner.EduSync.Speech.ServiceBus.Mediator;
//using Microsoft.Extensions.DependencyInjection;
//using System.Linq;
//using Xunit;

//namespace LogCorner.EduSync.Speech.ServiceBus.UnitTests
//{
//    public class ServicesConfigurationUnitTest
//    {
//        [Fact]
//        public void ShouldAddServiceBus()
//        {
//            //Arrange
//            IServiceCollection services = new ServiceCollection();

//            //Act
//            services.AddServiceBus("http://localhost:9092");
//            var contains = services.ToList();
//            var notifierMediatorService = contains.SingleOrDefault(c => c.ServiceType.Name == nameof(INotifierMediatorService) && c.Lifetime == ServiceLifetime.Transient);
//            var serviceBus = contains.SingleOrDefault(c => c.ServiceType.Name == nameof(IServiceBus) && c.Lifetime == ServiceLifetime.Singleton);
//            var serviceBusProvider = contains.SingleOrDefault(c => c.ServiceType.Name == nameof(IServiceBusProvider) && c.Lifetime == ServiceLifetime.Singleton);

//            //Assert
//            Assert.Equal(notifierMediatorService?.ImplementationType?.FullName, typeof(NotifierMediatorService).FullName);
//            Assert.Equal(serviceBus?.ImplementationType?.FullName, typeof(ServiceBus).FullName);
//            Assert.NotNull(serviceBusProvider);
//        }
//    }
//}