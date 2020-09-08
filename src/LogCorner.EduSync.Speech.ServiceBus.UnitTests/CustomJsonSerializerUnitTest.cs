using Xunit;

namespace LogCorner.EduSync.Speech.ServiceBus.UnitTests
{
    public class data
    {
        public data(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class CustomJsonSerializerUnitTest
    {
        [Fact]
        public void ServiceBusShouldSendMessageToKafka()
        {
            //Arrange

            //Act
            IJsonSerializer customJsonSerializer = new CustomJsonSerializer();
            var jsonString = customJsonSerializer.Serialize(new data(1));

            //Assert
            Assert.Equal(@"{'Id':1}", jsonString.Replace("\"", "'"));
        }
    }
}