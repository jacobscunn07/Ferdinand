using Ferdinand.Application.Commands.PublishOutboxMessage;
using Ferdinand.Application.Tests.Integration.TestUtils.Constants;
using Ferdinand.Domain.Events;
using Ferdinand.Domain.Primitives;
using Ferdinand.Infrastructure.Data.Outbox;

namespace Ferdinand.Application.Tests.Integration.Commands.TestUtils;

public static class PublishOutboxMessageCommandUtils
{
    public static PublishOutboxMessageCommand CreateCommand() => new();

    public static OutboxMessage CreateOutboxMessage(
        IDomainEvent? domainEvent = null)
    {
        domainEvent ??= new ColorAdded(
            Constants.Tenant.Name,
            Constants.Color.HexValue);

        return domainEvent.MapToOutboxMessage();
    }
}
