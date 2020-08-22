namespace LogCorner.EduSync.Speech.Producer
{
    public class Bus : IBus
    {
        private readonly IHubConnectionInstance _hubConnectionInstance;

        public  Bus (IHubConnectionInstance hubConnectionInstance)
        {
            _hubConnectionInstance = hubConnectionInstance;
        }

        public EventStore ReceiveAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}