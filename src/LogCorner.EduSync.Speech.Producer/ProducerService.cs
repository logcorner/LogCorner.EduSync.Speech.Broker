using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public class ProducerService : IProducerService
    {
        private readonly IServiceBus _serviceBus;

        public ProducerService(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public async Task DoWork()
        {
            await _serviceBus.ReceiveAsync();

            _serviceBus.ReceivedOnPublish += value =>
            {
            };

            await _serviceBus.Begin();
        }
    }
}