using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public class ProducerHostedService : BackgroundService
    {
        private readonly IProducerService _scopedProcessingService;

        public ProducerHostedService(IProducerService scopedProcessingService)
        {
            _scopedProcessingService = scopedProcessingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork();
        }

        private async Task DoWork()
        {
            await _scopedProcessingService.DoWork();
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}