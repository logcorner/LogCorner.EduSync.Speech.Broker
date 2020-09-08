using System.Text.Json;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class CustomJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T @event)
        {
            return JsonSerializer.Serialize(@event);
        }
    }
}