using Ferdinand.Application.Commands.PublishOutboxMessage;
using Ferdinand.Domain.Events;
using Ferdinand.Domain.Primitives;
using Ferdinand.Infrastructure.Data.Outbox;

namespace Ferdinand.Testing.Commands;

public static class PublishOutboxMessageCommandBuilder
{
    public static PublishOutboxMessageCommand CreateCommand() => new();

    public static OutboxMessage CreateOutboxMessage(
        IDomainEvent? domainEvent = null)
    {
        domainEvent ??= new ColorAdded(
            Constants.Tenant.Name,
            Constants.Color.HexValue.Black);

        return domainEvent.MapToOutboxMessage();
    }
}
