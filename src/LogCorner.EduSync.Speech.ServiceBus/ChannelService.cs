using System;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class ChannelService : IChannelService
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly IElasticSearchClient<SpeechView> _elasticSearchClient;
       
        public ChannelService(IEventSerializer eventSerializer,
            IElasticSearchClient<SpeechView> elasticSearchClient)
        {
            _eventSerializer = eventSerializer;
            _elasticSearchClient = elasticSearchClient;
           
        }
        public async Task Consume<T>(ChannelReader<T> reader, int partition) where T : class
        {
            Console.WriteLine($"CONSUMER : Starting on partition {partition}");

            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var data))

                {
                    Console.WriteLine($"CONSUMER : Consuming {data}");
                    Console.WriteLine($"on partition {partition}");

                    var eventStore = JsonSerializer.Deserialize<EventStore>(data.ToString());
                    var entity = _eventSerializer.Deserialize<Event>(eventStore.TypeName, eventStore.PayLoad);
                    var view = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();//new SpeechView();
                    view.LoadFromHistory(new IDomainEvent[] { entity });
                    await _elasticSearchClient.CreateAsync(view);

                    await Task.CompletedTask;
                }
            }

            Console.WriteLine($"CONSUMER : partition {partition} Completed");
        }

        public async Task Produce<T>(ChannelWriter<T> writer, int partition, T data) where T : class
        {
            Console.WriteLine($"PRODUCER : Starting on partition {partition}");

            Console.WriteLine($"PRODUCER partition {partition}");
            Console.WriteLine($"Creating {data}");

            await writer.WriteAsync(data);

            Console.WriteLine($"PRODUCER : Completed on partition {partition} ");
        }
    }
}