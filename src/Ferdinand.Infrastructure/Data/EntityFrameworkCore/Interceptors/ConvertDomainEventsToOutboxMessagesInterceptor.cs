using Ferdinand.Domain.Primitives;
using Ferdinand.Infrastructure.Data.Outbox;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Ferdinand.Infrastructure.Data.EntityFrameworkCore.Interceptors;

public class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is not FerdinandDbContext ctx)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = ctx.ChangeTracker
            .Entries<IHandleDomainEvents>()
            .Select(x => x.Entity)
            .SelectMany(x => x.Events)
            .Select(x => new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    CreatedUtc = DateTime.UtcNow,
                    Type = x.GetType().Name,
                    Content = JsonConvert.SerializeObject(x, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
                }
            )
            .ToList();
        
        ctx.OutboxMessages.AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
