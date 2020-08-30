using Confluent.Kafka;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class KafkaClient : IKafkaClient
    {
        private readonly IProducer<Null, string> _producer;

        private readonly IJsonSerializer _jsonSerializer;
        

        public KafkaClient(IProducer<Null, string> producer, IJsonSerializer jsonSerializer)
        {
            _producer = producer;
            _jsonSerializer = jsonSerializer;
          
        }

        public async Task SendAsync(string topic, EventStore @event)
        {
        

            var jsonString = _jsonSerializer.Serialize(@event);
            var t = _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonString });

            await t.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"error = {task.Exception?.Message}");
                }
                else
                {
                    Console.WriteLine($"produced : ");

                    Console.WriteLine($"Wrote to offset: {task.Result?.Offset}");
                }
            });
        }

    }
}