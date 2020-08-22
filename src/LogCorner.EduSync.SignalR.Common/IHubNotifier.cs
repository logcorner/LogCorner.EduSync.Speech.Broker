using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public interface IHubNotifier<T>
    {
        Task OnPublish(T payload);

        Task OnPublish(string topic, T payload);

        Task OnSubscribe(string connectionId, string topic);

        Task OnUnSubscribe(string connectionId, string topic);
    }
}