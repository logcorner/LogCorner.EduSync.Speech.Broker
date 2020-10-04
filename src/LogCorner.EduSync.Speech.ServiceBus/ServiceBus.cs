using System.Threading;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IServiceBusProvider _serviceBusProvider;

        public ServiceBus(IServiceBusProvider kafkaClient)
        {
            _serviceBusProvider = kafkaClient;
        }

        public async Task SendAsync(string topic, EventStore @event)
        {
            await _serviceBusProvider.SendAsync(topic, @event);
        }

        public async Task ReceiveAsync(string topic, CancellationToken stoppingToken)
        {
            await _kafkaClient.ReceiveAsync(topic, stoppingToken);
        }
    }
}