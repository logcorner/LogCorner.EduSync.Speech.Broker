using Xunit;

namespace LogCorner.EduSync.Speech.Producer.UnitTests
{
    public class BusUnitTest
    {
        [Fact]
        public void Should_receive_messages_to_produce()
        {
            IHubConnectionInstance hubConnectionInstance = new HubConnectionInstance();
            IBus bus = new Bus(hubConnectionInstance);

            var result = bus.ReceiveAsync();
        }
    }
}