using Confluent.Kafka;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class KafkaClient : IServiceBusProvider
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IConsumer<Null, string> _consumer;

        private readonly INotifierMediatorService _notifierMediatorService;

        public KafkaClient(IProducer<Null, string> producer, IJsonSerializer jsonSerializer,
            IConsumer<Null, string> consumer, INotifierMediatorService notifierMediatorService)
        {
            _producer = producer;
            _jsonSerializer = jsonSerializer;
            _consumer = consumer;
            _notifierMediatorService = notifierMediatorService;
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
                    Console.WriteLine($"produced : {jsonString}");

                    Console.WriteLine($"Wrote to offset: {task.Result?.Offset}");
                }
            });
        }

        public async Task ReceiveAsync(string topic, CancellationToken stoppingToken, bool forever = true)
        {
            _consumer.Subscribe(topic);
            Console.WriteLine($"consuming on topic {topic}");
            do
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    forever = false;
                }

                var data = _consumer.Consume();
                Console.WriteLine($"Key : {data.Message.Key}");
                Console.WriteLine($"Data : {data.Message.Value}");
                Console.WriteLine($"Partition : {data.Partition.Value}");
                Console.WriteLine($"Offset : {data.Offset.Value}");
                await _notifierMediatorService.Notify(data.Message.Value);
            } while (forever);
        }
    }
}