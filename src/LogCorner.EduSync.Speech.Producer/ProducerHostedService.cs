using Microsoft.Extensions.Hosting;
using System;
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

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _scopedProcessingService.StartAsync();
            Console.WriteLine("ProducerService is starting .....");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _scopedProcessingService.DoWorkAsync();
            Console.WriteLine("ProducerService is running .....");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _scopedProcessingService.StopAsync();
            Console.WriteLine("ProducerService is stopping .....");
            await Task.CompletedTask;
        }
    }
}