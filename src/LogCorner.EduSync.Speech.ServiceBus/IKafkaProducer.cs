using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IKafkaProducer
    {
        Task SendAsync(string topic, EventStore @event);
    }
}