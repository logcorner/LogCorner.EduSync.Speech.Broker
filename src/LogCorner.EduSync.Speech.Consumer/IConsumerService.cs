namespace LogCorner.EduSync.Speech.Consumer
{
    public interface IConsumerService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}