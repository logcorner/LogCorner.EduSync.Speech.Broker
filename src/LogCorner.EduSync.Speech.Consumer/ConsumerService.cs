using LogCorner.EduSync.Speech.ServiceBus;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Consumer
{
    public class ConsumerService : IConsumerService
    {
        private readonly IServiceBus _serviceBus;
 

        public ConsumerService(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
         
           
        }

        public async Task DoWorkAsync()
        {
            await _serviceBus.ReceiveAsync("speech");

           
        }
    }
}