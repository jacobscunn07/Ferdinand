using Ferdinand.Domain.Primitives;
using Newtonsoft.Json;

namespace Ferdinand.Infrastructure.Data.Outbox;

public static class DomainEventExtensions
{
    public static OutboxMessage MapToOutboxMessage(this IDomainEvent domainEvent)
    {
        return new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            CreatedUtc = DateTime.UtcNow,
            Type = domainEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            })
        };
    }
}
