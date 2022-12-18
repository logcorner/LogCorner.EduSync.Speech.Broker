using LogCorner.EduSync.Speech.Consumer;

namespace LogCorner.EduSync.Speech.Projection.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumerService _consumerService;

        public Worker(ILogger<Worker> logger, IConsumerService consumerService)
        {
            _logger = logger;
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerService.DoWorkAsync(stoppingToken);
            _logger.LogInformation("ConsumerService is running .....");
        }
    }
}