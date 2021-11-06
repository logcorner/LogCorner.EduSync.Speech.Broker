using Confluent.Kafka;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class KafkaClient : IServiceBusProvider
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IJsonSerializer _eventSerializer;

        private readonly IConsumer<Null, string> _consumer;

        private readonly INotifierMediatorService _notifierMediatorService;

        public KafkaClient(IProducer<Null, string> producer, IJsonSerializer eventSerializer,
            IConsumer<Null, string> consumer, INotifierMediatorService notifierMediatorService)
        {
            _producer = producer;
            _eventSerializer = eventSerializer;
            _consumer = consumer;
            _notifierMediatorService = notifierMediatorService;
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

        public async Task ReceiveAsync(string[] topics, CancellationToken stoppingToken, bool forever = true)
        {
            /* _consumer.Subscribe(topics);
             foreach (var topic in topics)
             {
                 Console.WriteLine($"**KafkaClient::ReceiveAsync - consuming on topic {topic}");
             }

             do
             {
                 if (stoppingToken.IsCancellationRequested)
                 {
                     forever = false;
                 }

                 var data = _consumer.Consume();
                 Console.WriteLine($"**KafkaClient::ReceiveAsync - key : {data.Message.Key}");

                 Console.WriteLine($"**KafkaClient::ReceiveAsync - partition : {data.Partition.Value}");
                 Console.WriteLine($"**KafkaClient::ReceiveAsync - offset : {data.Offset.Value}");
                 var message = new NotificationMessage<string> { Message = data.Message.Value };
                 Console.WriteLine($"**KafkaClient::ReceiveAsync - message : {message.Message}");
                 await _notifierMediatorService.Notify(message);
             } while (forever);*/

            try
            {
                _consumer.Subscribe(topics);
                Console.WriteLine($"**KafkaClient::ReceiveAsync - consuming on topic {string.Join(' ', topics)}");
                while (true)
                {
                    var data = _consumer.Consume();

                    if (data?.Message?.Value == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"**KafkaClient::ReceiveAsync - key : {data.Message.Key}");

                    Console.WriteLine($"**KafkaClient::ReceiveAsync - partition : {data.Partition.Value}");
                    Console.WriteLine($"**KafkaClient::ReceiveAsync - offset : {data.Offset.Value}");
                    var message = new NotificationMessage<string> { Message = data.Message.Value };
                    Console.WriteLine($"**KafkaClient::ReceiveAsync - message : {message.Message}");
                    await _notifierMediatorService.Notify(message);

                    _consumer.Commit(data);
                    _consumer.StoreOffset(data);
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
            catch (KafkaException e)
            {
                Console.WriteLine($"Consume error: {e.Message}");
                Console.WriteLine("Exiting producer...");
            }
            finally
            {
                _consumer.Close();
            }
        }
    }
}