using System.Threading.Tasks;
using LogCorner.EduSync.Speech.SharedKernel.Events;

namespace LogCorner.EduSync.Speech.Producer
{
    public interface IServiceBus
    {
        Task SendAsync(string topic, EventStore @event);
    }
}