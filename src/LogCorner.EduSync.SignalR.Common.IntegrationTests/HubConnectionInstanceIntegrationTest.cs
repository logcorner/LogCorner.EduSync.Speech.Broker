using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.SignalR.Common.IntegrationTests
{
    public class HubConnectionInstanceIntegrationTest
    {
        [Fact]
        public void ShouldInitHubConnectionInstance()
        {
            //Arrange
            IHubInstance hubConnectionInstance = new HubConnectionInstance("http://localhost/logcornerhub",null);

            //Act
            var t = hubConnectionInstance.InitAsync();

            //Assert
            Assert.NotNull(hubConnectionInstance.Connection);
            Assert.True(hubConnectionInstance.Connection.State == HubConnectionState.Disconnected);
            Assert.Equal(TaskStatus.RanToCompletion, t.Status);
        }

        [Fact]
        public void ShouldStartHubConnectionInstance()
        {
            //Arrange
            IHubInstance hubConnectionInstance = new HubConnectionInstance("http://localhost/logcornerhub",null);
            var t = hubConnectionInstance.InitAsync();

            //Act
            hubConnectionInstance.StartAsync();

            //Assert
            Assert.NotNull(hubConnectionInstance.Connection);
            Assert.True(hubConnectionInstance.Connection.State == HubConnectionState.Connecting);
            Assert.Equal(TaskStatus.RanToCompletion, t.Status);
        }
    }
}