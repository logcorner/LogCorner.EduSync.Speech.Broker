using Polly;

namespace LogCorner.EduSync.Speech.Resiliency
{
    public interface IResiliencyService
    {
        public IAsyncPolicy ExponentialExceptionRetry { get; set; }
    }
}