using Bogus;
using Ferdinand.Application.Commands.PublishOutboxMessage;
using Ferdinand.Common.Logging;
using Ferdinand.Common.Messaging;
using Ferdinand.Data.EntityFrameworkCore;
using Ferdinand.Data.EntityFrameworkCore.Repositories;
using Ferdinand.Data.Outbox;
using Ferdinand.Domain.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Commands.PublishOutboxMessage;

public class PublishOutboxCommandHandlerTests : IClassFixture<HostFixture>
{
    private readonly HostFixture _fixture;

    public PublishOutboxCommandHandlerTests(HostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_ShouldPublishOutboxMessages_WhenMessagesExist()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var messageRepository = scope.ServiceProvider.GetRequiredService<OutboxMessageRepository>();
        var outboxMessage = GetOutboxMessage();
        await messageRepository.AddRange(new[] { outboxMessage });
        await ctx.SaveChangesAsync();
        var logger = Substitute.For<ILoggerAdapter<PublishOutboxMessageCommandHandler>>();
        var bus = Substitute.For<IMessageBus>();
        bus.Publish(Arg.Any<object>()).Returns(Task.CompletedTask);
        var command = new PublishOutboxMessageCommand();
        var cancellationToken = new CancellationToken();
        var sut = new PublishOutboxMessageCommandHandler(messageRepository, bus, logger);

        // Act
        await sut.Handle(command, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);

        // Assert
        await bus.Received(1).Publish(Arg.Any<object>());
        logger.Received(1).LogInformation(Arg.Any<string?>(), Arg.Any<object?[]>());
        (messageRepository.GetAll().SingleOrDefault(x => x.Id == outboxMessage.Id))?.ProcessedUtc.Should().NotBeNull();
    }

    private static OutboxMessage GetOutboxMessage()
    {
        var domainEvent = new ColorAdded(
            new Faker().Company.CompanyName(),
            new Randomizer().Hexadecimal(6, ""));
        var outboxMessage = new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            CreatedUtc = DateTime.UtcNow,
            Type = domainEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            })
        };
        return outboxMessage;
    }
}
