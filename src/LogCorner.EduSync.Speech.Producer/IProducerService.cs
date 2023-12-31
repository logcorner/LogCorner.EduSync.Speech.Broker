using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public interface IProducerService
    {
        Task ProduceAsync(string topic, string @event);
    }
}