using System.Text.Json;
using Ferdinand.Data.Outbox;
using Ferdinand.Domain.Primitives;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ferdinand.Data.EntityFrameworkCore.Interceptors;

public class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var ctx = eventData.Context;

        if (ctx is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = ctx.ChangeTracker
            .Entries<AggregateRoot<IValueObject>>()
            .Select(x => x.Entity)
            .SelectMany(x => x.Events)
            .Select(x => new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    CreatedUtc = DateTime.UtcNow,
                    Type = x.GetType().Name,
                    Content = JsonSerializer.Serialize(x)
                }
            )
            .ToList();
        
        ctx.Set<OutboxMessage>().AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
