namespace Ferdinand.Common.Messaging;

public interface IMessageBus
{
    Task Publish(object message);
}
