using LogCorner.EduSync.Speech.SharedKernel.Events;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IKafkaClient _kafkaClient;

        public ServiceBus(IKafkaClient kafkaClient)
        {
            _kafkaClient = kafkaClient;
        }

        public async Task SendAsync(string topic, EventStore @event)
        {
            await _kafkaClient.SendAsync(topic, @event);
        }

        public async Task ReceiveAsync(string topic)
        {
            await _kafkaClient.ReceiveAsync(topic);
        }
    }
}