using LogCorner.EduSync.Speech.SharedKernel.Events;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public interface IServiceBus
    {
        Task SendAsync(string topic, EventStore @event);
    }
}