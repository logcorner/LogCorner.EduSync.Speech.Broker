using LogCorner.EduSync.Speech.SharedKernel.Events;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IServiceBusProvider
    {
        Task SendAsync(string topic, EventStore @event);
    }
}