using Ferdinand.Domain.Primitives;
using Ferdinand.Infrastructure.Data.Outbox;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
            .Select(x => x.MapToOutboxMessage())
            .ToList();
        
        ctx.OutboxMessages.AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
