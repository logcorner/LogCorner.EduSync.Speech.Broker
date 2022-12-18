using LogCorner.EduSync.Speech.ServiceBus;

namespace LogCorner.EduSync.Speech.Consumer;

public class ConsumerService : IConsumerService
{
    private readonly IServiceBusReceiver _serviceBus;
    private readonly IKafkaClusterManager _clusterManager;

    public ConsumerService(IServiceBusReceiver serviceBus, IKafkaClusterManager clusterManager)
    {
        _serviceBus = serviceBus;
        _clusterManager = clusterManager;
    }

    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        var topics = new[] { "speech", "synchro" };

        foreach (var topic in topics)
        {
            await _clusterManager.EnsureTopicExistAsync(topic);
        }

        await _serviceBus.ReceiveAsync(topics, stoppingToken);
    }
}