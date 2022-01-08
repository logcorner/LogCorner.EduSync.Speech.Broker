using Confluent.Kafka;
using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using LogCorner.EduSync.Speech.Command.SharedKernel.Serialyser;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IJsonSerializer _eventSerializer;

        public KafkaProducer(IProducer<Null, string> producer, IJsonSerializer eventSerializer)
        {
            _producer = producer;
            _eventSerializer = eventSerializer;
        }

        public async Task SendAsync(string topic, EventStore @event)
        {
            var jsonString = _eventSerializer.Serialize(@event);
            var t = _producer.ProduceAsync(topic, new Message<Null, string>
            { Value = jsonString });
            Console.WriteLine($"**KafkaClient::SendAsync - jsonString = {jsonString}");
            await t.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"**KafkaClient::SendAsync - error = {task.Exception?.Message}");
                }
                else
                {
                    Console.WriteLine($"**KafkaClient::SendAsync - produced : {@event?.Id} - {@event?.Name}");

                    Console.WriteLine($"**KafkaClient::SendAsync - wrote to offset: {task.Result?.Offset}");
                }
            });
        }
    }
}