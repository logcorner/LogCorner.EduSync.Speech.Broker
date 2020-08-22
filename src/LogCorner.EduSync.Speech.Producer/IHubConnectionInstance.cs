using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public interface IHubConnectionInstance
    {
        Task ConnectAsync();
    }
}