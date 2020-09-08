using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Consumer
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IConsumerService _consumerService;

        public ConsumerHostedService(IConsumerService scopedProcessingService)
        {
            _consumerService = scopedProcessingService;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("ConsumerService is starting .....");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerService.DoWorkAsync();
            Console.WriteLine("ConsumerService is running .....");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("ConsumerService is stopping .....");
            await Task.CompletedTask;
        }
    }
}