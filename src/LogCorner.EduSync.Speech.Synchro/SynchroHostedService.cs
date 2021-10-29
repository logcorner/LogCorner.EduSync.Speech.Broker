using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Synchro
{
    public class SynchroHostedService : BackgroundService
    {
        private readonly ISynchroService _scopedProcessingService;

        public SynchroHostedService(ISynchroService scopedProcessingService)
        {
            _scopedProcessingService = scopedProcessingService;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _scopedProcessingService.StartAsync();
            Console.WriteLine("SynchroService is starting .....");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _scopedProcessingService.DoWorkAsync();
            Console.WriteLine("SynchroService is running .....");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _scopedProcessingService.StopAsync();
            Console.WriteLine("SynchroService is stopping .....");
            await Task.CompletedTask;
        }
    }
}