namespace LogCorner.EduSync.Speech.ServiceBus.Mediator;

public interface INotifierMediatorService
{
    Task Notify<T>(NotificationMessage<T> notifyText);
}