using LogCorner.EduSync.Speech.Command.SharedKernel.Events;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IServiceBusProvider _serviceBusProvider;

        public ServiceBus(IServiceBusProvider serviceBusProvider)
        {
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task SendAsync(string topic, EventStore @event)
        {
            await _serviceBusProvider.SendAsync(topic, @event);
        }

        public async Task ReceiveAsync(string[] topics, CancellationToken stoppingToken)
        {
            await _serviceBusProvider.ReceiveAsync(topics, stoppingToken);
        }
    }
}