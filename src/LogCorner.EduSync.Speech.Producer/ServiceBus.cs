using Confluent.Kafka;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public class ServiceBus : IServiceBus
    {
        private readonly ProducerConfig _config;

        public ServiceBus(string url)
        {
            _config = new ProducerConfig { BootstrapServers = url };
        }

        public async Task SendAsync(string topic, EventStore @event)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var jsonString = JsonSerializer.Serialize(@event);
            var t = producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonString });

            await t.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine($"error = {task.Exception.Message}");
                }
                else
                {
                    Console.WriteLine($"produced : ");

                    Console.WriteLine($"Wrote to offset: {task.Result.Offset}");
                }
            });
        }
    }
}