namespace Ferdinand.Infrastructure.Messaging;

public interface IMessageBus
{
    Task Publish(object message);
}
