using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public interface IProducerService
    {
        Task DoWorkAsync();

        Task StopAsync();

        Task StartAsync();
    }
}