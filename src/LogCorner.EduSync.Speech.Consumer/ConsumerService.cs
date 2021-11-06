using LogCorner.EduSync.Speech.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Consumer
{
    public class ConsumerService : IConsumerService
    {
        private readonly IServiceBus _serviceBus;
        private readonly IClusterManager _clusterManager;

        public ConsumerService(IServiceBus serviceBus, IClusterManager clusterManager)
        {
            _serviceBus = serviceBus;
            _clusterManager = clusterManager;
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            var topics = new[] { Topics.Speech, Topics.Synchro };
            try
            {
                foreach (var topic in topics)
                {
                    await _clusterManager.EnsureTopicExistAsync(topic);
                }

                await _serviceBus.ReceiveAsync(topics, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ConsumerService::DoWorkAsync:errorMessage - {ex.Message} ");
            }
        }
    }
}