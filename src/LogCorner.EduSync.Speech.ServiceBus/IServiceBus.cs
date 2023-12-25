using LogCorner.EduSync.Speech.Command.SharedKernel.Events;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IServiceBus
    {
        Task SendAsync(string topic, EventStore @event);

        Task ReceiveAsync(string[] topics, CancellationToken stoppingToken);
    }
}