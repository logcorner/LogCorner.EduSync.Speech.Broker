using System;

namespace LogCorner.EduSync.Speech.Producer
{
    public interface IServiceBus
    {
        event Action<EventStore> ReceivedOnPublish;
        void ReceiveAsync();
    }
}