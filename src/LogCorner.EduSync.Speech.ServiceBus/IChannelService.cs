using System.Threading.Channels;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IChannelService
    {
       
        Task Consume<T>(ChannelReader<T> channelReader, int partition) where T : class;

        Task Produce<T>(ChannelWriter<T> channelWriter, int partition, T data) where T : class;
    }

    
}