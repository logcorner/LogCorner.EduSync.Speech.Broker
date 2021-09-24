using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class KafkaClusterManager : IClusterManager
    {
        private readonly IConfiguration _configuration;

        public KafkaClusterManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnsureTopicExistAsync(string topicName)
        {
            var bootstrapServers = _configuration["kafkaUrl"];
            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
            try
            {
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                var topicExist = metadata.Topics.Any(a => a.Topic == topicName);
                if (topicExist == false)
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
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }
}