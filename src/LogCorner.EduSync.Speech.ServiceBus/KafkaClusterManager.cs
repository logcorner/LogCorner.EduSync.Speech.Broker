using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LogCorner.EduSync.Speech.ServiceBus;

public class KafkaClusterManager : IKafkaClusterManager
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaClusterManager> _logger;

    public KafkaClusterManager(IConfiguration configuration, ILogger<KafkaClusterManager> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task EnsureTopicExistAsync(string topicName)
    {
        var bootstrapServers = _configuration["kafkaUrl"];
        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        try
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var topicExist = metadata.Topics.Any(a => a.Topic == topicName);
            if (!topicExist)
            {
                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification
                    {
                        Name = topicName,
                        ReplicationFactor = 1,
                        NumPartitions = 1
                    }
                });
            }
        }
        catch (CreateTopicsException e)
        {
            _logger.LogInformation($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        }
    }
}