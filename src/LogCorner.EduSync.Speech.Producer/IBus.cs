namespace LogCorner.EduSync.Speech.Producer
{
    public interface IBus
    {
        EventStore ReceiveAsync();
    }
}