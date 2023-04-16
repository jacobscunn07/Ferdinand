using MediatR;

namespace Ferdinand.Application.Commands.PublishOutboxMessage;

public record PublishOutboxMessageCommand() : IRequest;
