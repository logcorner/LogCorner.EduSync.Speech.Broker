using System.Threading.Tasks;
using LogCorner.EduSync.SignalR.Common;

namespace LogCorner.EduSync.Speech.Producer
{
    public class ProducerService : IProducerService
    {
        private readonly ISignalRNotifier _notifier;
        private readonly ISignalRPublisher _publisher;
       
        public ProducerService(ISignalRNotifier notifier, ISignalRPublisher publisher)
        {
            _notifier = notifier;
            _publisher = publisher;
        }

        public async Task DoWork()
        {
            await _publisher.SubscribeAsync("speech");

            await _notifier.OnPublish("speech");

            _notifier.ReceivedOnPublishToTopic += (topic, @event) =>
            {
               
            };
        }
    }
}