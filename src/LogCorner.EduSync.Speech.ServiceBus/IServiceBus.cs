using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IServiceBus
    {
        Task SendAsync(string topic, EventStore @event);

        Task ReceiveAsync(string[] topics, CancellationToken stoppingToken);
    }
}