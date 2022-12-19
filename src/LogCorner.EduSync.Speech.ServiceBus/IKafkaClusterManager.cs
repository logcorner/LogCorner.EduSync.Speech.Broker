namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IKafkaClusterManager
    {
        Task EnsureTopicExistAsync(string topicName);
    }
}