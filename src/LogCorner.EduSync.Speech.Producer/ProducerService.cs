using LogCorner.EduSync.SignalR.Common;
using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
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

        public async Task StartAsync()
        {
            await _notifier.StartAsync();
        }

        public async Task StopAsync()
        {
            await _notifier.StopAsync();
        }

        public async Task DoWorkAsync()
        {
            await _publisher.SubscribeAsync(Topics.Speech);

            await _notifier.OnPublish(Topics.Speech);

            _notifier.ReceivedOnPublishToTopic += async (topic, @event) =>
            {
                Console.WriteLine($"**ProducerService::DoWorkAsync - topic : {topic},@event : {@event} ");
                if (@event is EventStore output)
                {
                    Console.WriteLine($"**ProducerService::DoWorkAsync - topic : {Topics.Speech},output : {output} ");
                    await _serviceBus.SendAsync(Topics.Speech, output);
                }
            };
        }
    }
}