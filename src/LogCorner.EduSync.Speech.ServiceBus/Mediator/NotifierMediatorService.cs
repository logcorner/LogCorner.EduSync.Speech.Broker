using System.Threading.Tasks;
using MediatR;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator
{
    public class NotifierMediatorService : INotifierMediatorService
    {
        private readonly IMediator _mediator;

        public NotifierMediatorService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Notify(string notifyText)
        {
          await  _mediator.Publish(new NotificationMessage<string> { Message = notifyText });
        }
    }
}