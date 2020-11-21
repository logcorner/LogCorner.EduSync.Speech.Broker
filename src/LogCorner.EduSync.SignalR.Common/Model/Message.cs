namespace LogCorner.EduSync.SignalR.Common.Model
{
    public class Message
    {
        public Message(string type, string body)
        {
            Type = type;
            Body = body;
        }

        public string Type { get; }

        public string Body { get; }
    }
}