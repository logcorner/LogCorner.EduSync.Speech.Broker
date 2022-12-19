using Polly;
using System;

namespace LogCorner.EduSync.Speech.Resiliency
{
    public class ResiliencyService : IResiliencyService
    {
        public IAsyncPolicy ExponentialExceptionRetry { get; set; }

        public ResiliencyService()
        {
            ExponentialExceptionRetry = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),

                    (exception, retryCount, context) =>
                    {
                        Console.WriteLine($"EventSourcingHandler::Handle:TryConnect {retryCount}, Exception: {exception.Message}, {context.CorrelationId}");
                    });
        }
    }
}