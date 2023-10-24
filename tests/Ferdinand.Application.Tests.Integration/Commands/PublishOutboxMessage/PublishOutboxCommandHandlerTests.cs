using Ferdinand.Application.Commands.PublishOutboxMessage;
using Ferdinand.Application.Tests.Integration.Commands.TestUtils;
using Ferdinand.Infrastructure.Data.Outbox;
using Ferdinand.Infrastructure.Logging;
using Ferdinand.Infrastructure.Messaging;
using FluentAssertions;
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

    [Theory]
    [MemberData(nameof(Handle_ShouldPublishOutboxMessages_WhenMessagesExist_TestCases))]
    public async Task Handle_ShouldPublishOutboxMessages_WhenMessagesExist(
        PublishOutboxMessageCommand command,
        OutboxMessage outboxMessage)
    {
        // Arrange
        await _fixture.OutboxMessageRepository.AddRange(new[] { outboxMessage });
        await _fixture.FerdinandDbContext.SaveChangesAsync();
        var logger = Substitute.For<ILoggerAdapter<PublishOutboxMessageCommandHandler>>();
        var bus = Substitute.For<IMessageBus>();
        bus.Publish(Arg.Any<object>()).Returns(Task.CompletedTask);
        var cancellationToken = new CancellationToken();
        var sut = new PublishOutboxMessageCommandHandler(_fixture.OutboxMessageRepository, bus, logger);

        // Act
        await sut.Handle(command, cancellationToken);
        await _fixture.FerdinandDbContext.SaveChangesAsync(cancellationToken);

        // Assert
        await bus.Received(1).Publish(Arg.Any<object>());
        logger.Received(1).LogInformation(Arg.Any<string?>(), Arg.Any<object?[]>());
        (_fixture.OutboxMessageRepository.GetAll().SingleOrDefault(x => x.Id == outboxMessage.Id))?.ProcessedUtc.Should().NotBeNull();
    }
    
    public static IEnumerable<object[]> Handle_ShouldPublishOutboxMessages_WhenMessagesExist_TestCases()
    {
        yield return new object[]
        {
            PublishOutboxMessageCommandUtils.CreateCommand(),
            PublishOutboxMessageCommandUtils.CreateOutboxMessage()
        };
    }
}
