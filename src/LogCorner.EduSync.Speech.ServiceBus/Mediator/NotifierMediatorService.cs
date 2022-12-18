using LogCorner.EduSync.Speech.Resiliency;
using MediatR;
using System.Diagnostics;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator;

public class NotifierMediatorService : INotifierMediatorService
{
    private readonly IMediator _mediator;
    private readonly IResiliencyService _resiliencyService;
    private static readonly ActivitySource Activity = new("consumer-service");

    public NotifierMediatorService(IMediator mediator, IResiliencyService resiliencyService)
    {
        _mediator = mediator;
        _resiliencyService = resiliencyService;
    }

    public async Task Notify<T>(NotificationMessage<T> notifyText)
    {
        ArgumentNullException.ThrowIfNull(notifyText);
        // using var activity = Activity.StartActivity("Publish Message to Mediator", ActivityKind.Producer);
        //await _resiliencyService.ExponentialExceptionRetry.ExecuteAsync(async () => await _mediator.Publish(notifyText));

        await _mediator.Publish(notifyText);
    }
}