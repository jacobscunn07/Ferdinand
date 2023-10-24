using Ferdinand.Domain.Events;
using Ferdinand.Domain.Primitives;
using Ferdinand.Infrastructure.Data.Outbox;

namespace Ferdinand.Application.Tests.Integration.TestUtils.OutboxMessages.Extensions;

public static partial class OutboxMessageExtensions
{
    public static OutboxMessage CreateOutboxMessage(
        IDomainEvent? domainEvent = null)
    {
        domainEvent ??= new ColorAdded(
            Constants.Constants.Tenant.Name,
            Constants.Constants.Color.HexValue);

        var outboxMessage = domainEvent.MapToOutboxMessage();
        
        return outboxMessage;
    }
}
