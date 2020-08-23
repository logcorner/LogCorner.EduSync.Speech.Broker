using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public interface IServiceBus
    {
        event Action<EventStore> ReceivedOnPublish;

        Task ReceiveAsync();

        Task Begin();
    }
}