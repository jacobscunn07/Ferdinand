using Ferdinand.Common.Logging;
using Ferdinand.Common.Messaging;
using Ferdinand.Data.EntityFrameworkCore.Repositories;
using Ferdinand.Domain.Primitives;
using MediatR;
using Newtonsoft.Json;

namespace Ferdinand.Application.Commands.PublishOutboxMessage;

public class PublishOutboxMessageCommandHandler : IRequestHandler<PublishOutboxMessageCommand>
{
    private readonly OutboxMessageRepository _outboxMessageRepository;
    private readonly IMessageBus _bus; 
    private readonly ILoggerAdapter<PublishOutboxMessageCommandHandler> _logger;

    public PublishOutboxMessageCommandHandler(OutboxMessageRepository outboxMessageRepository, IMessageBus bus, ILoggerAdapter<PublishOutboxMessageCommandHandler> logger)
    {
        _outboxMessageRepository = outboxMessageRepository;
        _bus = bus;
        _logger = logger;
    }

    public async Task<Unit> Handle(PublishOutboxMessageCommand request, CancellationToken cancellationToken)
    {
        var outboxMessages = _outboxMessageRepository.GetAll()
            .Where(x => x.ProcessedUtc == null)
            .OrderByDescending(x => x.CreatedUtc)
            .ToList();
        
        _logger.LogInformation("{Name} found {Count} outbox messages to publish", GetType().Name, outboxMessages.Count);

        if (!outboxMessages.Any()) return Unit.Value;

        foreach (var outboxMessage in outboxMessages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                outboxMessage.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            if (domainEvent is null) continue;
            
            await _bus.Publish(domainEvent);
            
            outboxMessage.ProcessedUtc = DateTime.UtcNow;
        }
        
        return Unit.Value;
    }
}
