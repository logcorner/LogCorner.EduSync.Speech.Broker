using LogCorner.EduSync.SignalR.Common;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    //kafka-topics --zookeeper 127.0.0.1:2181  --topic eventbus  --create  --partitions 3  --replication-factor 1

    //kafka-console-consumer --bootstrap-server 127.0.0.1:9092  --topic eventbus  --from-beginning
    public class ProducerService : IProducerService
    {
        private readonly ISignalRNotifier _notifier;
        private readonly ISignalRPublisher _publisher;
        private readonly IServiceBus _serviceBus;

        public ProducerService(ISignalRNotifier notifier, ISignalRPublisher publisher, IServiceBus serviceBus)
        {
            _notifier = notifier;
            _publisher = publisher;
            _serviceBus = serviceBus;
        }

        public async Task DoWork()
        {
            await _publisher.SubscribeAsync("speech");

            await _notifier.OnPublish("speech");

            _notifier.ReceivedOnPublishToTopic += async (topic, @event) =>
            {
                await _serviceBus.SendAsync("eventbus", @event);
            };
        }
    }
}