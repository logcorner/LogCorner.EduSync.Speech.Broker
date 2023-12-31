using LogCorner.EduSync.Speech.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public class ProducerService : IProducerService
    {
        private readonly IServiceBusProducer _serviceBus;

        private readonly ILogger<ProducerService> _logger;

        public ProducerService(IServiceBusProducer serviceBus, ILogger<ProducerService> logger)
        {
            _serviceBus = serviceBus;
            _logger = logger;
        }

        public async Task ProduceAsync(string topic, string @event)
        {
            _logger.LogInformation($@"**ProducerService::DoWorkAsync - topic : {topic},@event : {@event} ");

            await _serviceBus.SendAsync(topic, @event);
        }
    }
}