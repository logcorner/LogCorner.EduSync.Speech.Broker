using MediatR;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator
{
    public class NotifierMediatorService : INotifierMediatorService
    {
        private readonly IMediator _mediator;

        public NotifierMediatorService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Notify<T>(NotificationMessage<T> notifyText)
        {
            if (notifyText == null)
            {
                throw new ArgumentNullException(nameof(notifyText));
            }
            await _mediator.Publish(notifyText);
        }
    }
}