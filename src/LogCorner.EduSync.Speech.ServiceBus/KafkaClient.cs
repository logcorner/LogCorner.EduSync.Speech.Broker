using Confluent.Kafka;
using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using LogCorner.EduSync.Speech.Command.SharedKernel.Serialyser;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using LogCorner.EduSync.Speech.Telemetry;
using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly ITraceService _traceService;

        public KafkaClient(IProducer<Null, string> producer, IJsonSerializer eventSerializer,
            IConsumer<Null, string> consumer, INotifierMediatorService notifierMediatorService, ITraceService traceService)
        {
            _producer = producer;
            _eventSerializer = eventSerializer;
            _consumer = consumer;
            _notifierMediatorService = notifierMediatorService;
            _traceService = traceService;
        }

        public async Task SendAsync(string topic, EventStore @event)
        {
            using (var activity = _traceService.StartActivity($"ProducerService::DoWorkAsync - topic : {topic}"))
            {
                var headers = new Dictionary<string, object>();
                _traceService.InjectContextIntoHeader(activity, headers);
                var kafkaHeaders = new Headers();

                foreach (var item in headers)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(item.Value.ToString() ?? string.Empty);
                    kafkaHeaders.Add(item.Key, bytes);
                }
                IDictionary<string, object> tags = new Dictionary<string, object>
                {
                    {"messaging.system", "kafka"},
                    {"messaging.destination_kind", "queue"},
                    {"messaging.rabbitmq.queue", "sample"}
                };
                _traceService.AddActivityTags(activity, tags);
                var jsonString = _eventSerializer.Serialize(@event);
                var t = _producer.ProduceAsync(topic, new Message<Null, string>
                { Value = jsonString, Headers = kafkaHeaders });

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

                    IDictionary<string, object> headers = new Dictionary<string, object>();

                    foreach (var item in data.Message.Headers)
                    {
                        var bytes = item.GetValueBytes();
                        if (bytes != null)
                        {
                            var value = Encoding.UTF8.GetString(bytes);
                            headers.Add(item.Key, value);
                            Console.WriteLine(
                                $"**KafkaClient::ReceiveAsync - Headers : [{item.Key},{value}]");
                        }
                    }
                    using (var activity = _traceService.StartActivity("Process Message", headers))
                    {
                        if (data?.Message?.Value == null)
                        {
                            continue;
                        }
                        IDictionary<string, object> tags = new Dictionary<string, object>
                        {
                            {"messaging.system", "kafka"},
                            {"messaging.destination_kind", "queue"},
                            {"messaging.rabbitmq.queue", "sample"}
                        };
                        _traceService.AddActivityTags(activity, tags);

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