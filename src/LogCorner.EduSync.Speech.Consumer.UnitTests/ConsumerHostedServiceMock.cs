using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Consumer.UnitTests
{
    public class ConsumerHostedServiceMock : ConsumerHostedService
    {
        public ConsumerHostedServiceMock(IConsumerService scopedProcessingService) : base(scopedProcessingService)
        {
        }

        public new async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await base.ExecuteAsync(cancellationToken);
        }
    }
}