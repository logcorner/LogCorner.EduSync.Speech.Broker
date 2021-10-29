using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IClusterManager
    {
        Task EnsureTopicExistAsync(string topicName);
    }
}