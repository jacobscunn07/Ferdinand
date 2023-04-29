using NServiceBus;

namespace Ferdinand.Infrastructure.Messaging;

public class NServiceBusMessageBus : IMessageBus
{
    private readonly IMessageSession _messageSession;
    
    public NServiceBusMessageBus(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }
    
    public Task Publish(object message)
    {
        return _messageSession.Publish(message);
    }
}
