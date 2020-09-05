using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class ChannelService : IChannelService
    {
        public async Task Consume<T>(ChannelReader<T> reader, int partition) where T : class
        {
            Console.WriteLine($"CONSUMER : Starting on partition {partition}");

            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var data))

                {
                    Console.WriteLine($"CONSUMER : Consuming {data}");
                    Console.WriteLine($"on partition {partition}");
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