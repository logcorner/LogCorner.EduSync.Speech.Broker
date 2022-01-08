using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IServiceBusProvider
    {
        Task SendAsync(string topic, EventStore @event);

        Task ReceiveAsync(string[] topic, CancellationToken stoppingToken, bool runAlawys = true);
    }
}