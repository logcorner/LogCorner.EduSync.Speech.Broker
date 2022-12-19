namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IServiceBusProducer
    {
        Task SendAsync(string topic, string @event);
    }
}