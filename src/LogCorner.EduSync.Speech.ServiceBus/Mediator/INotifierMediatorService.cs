using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator
{
    public interface INotifierMediatorService
    {
        Task Notify(string notifyText);
    }
}