using MediatR;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator;

public class NotificationMessage<T> : INotification
{
    public T Message { get; set; }
}