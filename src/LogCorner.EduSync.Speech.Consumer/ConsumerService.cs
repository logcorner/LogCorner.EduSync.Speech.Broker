using System.Threading;
using LogCorner.EduSync.Speech.ServiceBus;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Consumer
{
    public class ConsumerService : IConsumerService
    {
        private readonly IServiceBus _serviceBus;

        public ConsumerService(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            await _serviceBus.ReceiveAsync(Topics.Speech, stoppingToken);
        }
    }
}