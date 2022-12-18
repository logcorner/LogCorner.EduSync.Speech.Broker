namespace LogCorner.EduSync.Speech.ServiceBus;

public interface IServiceBusReceiver
{
    Task ReceiveAsync(string[] topics, CancellationToken stoppingToken, bool runAlways = true);
}