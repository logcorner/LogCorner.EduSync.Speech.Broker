using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace LogCorner.EduSync.SignalR.Common
{
    public interface IHubConnectionInstance
    {
       
        HubConnection Connection { get; }
        Task ConnectAsync();
    }
}