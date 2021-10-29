using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Synchro
{
    public interface ISynchroService
    {
        Task DoWorkAsync();

        Task StopAsync();

        Task StartAsync();
    }
}