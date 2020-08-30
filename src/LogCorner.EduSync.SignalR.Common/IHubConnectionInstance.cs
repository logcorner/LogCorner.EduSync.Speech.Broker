using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public interface IHubConnectionInstance
    {
        HubConnection Connection { get; }

        Task InitAsync();
        Task StartAsync();
    }
}