using Ferdinand.Application.Commands.AddColor;
using Ferdinand.Domain;
using Ferdinand.Domain.Models;
using Ferdinand.Testing;
using Ferdinand.Testing.Builders;
using Ferdinand.Testing.Extensions;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Commands.AddColor;

public class AddColorCommandHandlerTests : IClassFixture<HostFixture>
{
    private readonly HostFixture _fixture;

    public AddColorCommandHandlerTests(HostFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [MemberData(nameof(Handle_ShouldAddColor_WhenInvokedWithValidInputs_TestCases))]
    public async Task Handle_ShouldAddColor_WhenInvokedWithValidInputs(AddColorCommand command)
    {
        // Arrange
        var sut = new AddColorCommandHandler(_fixture.ColorRepository);

        // Act
        var result = await sut.Handle(command, new CancellationToken());
        await _fixture.FerdinandDbContext.SaveChangesAsync();

        // Assert
        (await _fixture.ColorRepository.Exists(ColorKey.Create(result.Key))).Should().BeTrue();
        (await _fixture.ColorRepository.GetByKey(ColorKey.Create(result.Key))).ValidateCreatedFrom(command);
    }

    public static IEnumerable<object[]> Handle_ShouldAddColor_WhenInvokedWithValidInputs_TestCases()
    {
        yield return new object[] { AddColorCommandBuilder.CreateCommand() };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(tenant: "ABC", description: "") };
    }

    [Theory]
    [MemberData(nameof(Handle_ShouldThrow_WhenInvokedWithInvalidInputs_TestCases))]
    public async Task Handle_ShouldThrow_WhenInvokedWithInvalidInputs(AddColorCommand command)
    {
        // Arrange
        var sut = new AddColorCommandHandler(_fixture.ColorRepository);

        // Act
        var action = async () => await sut.Handle(command, new CancellationToken());

        // Assert
        await action.Should().ThrowAsync<DomainException>();
    }

    public static IEnumerable<object[]> Handle_ShouldThrow_WhenInvokedWithInvalidInputs_TestCases()
    {
        yield return new object[]
        {
            AddColorCommandBuilder.CreateCommand(
                tenant: Constants.Tenant.Name.Value,
                hexValue: Constants.Color.HexValue.Invalid)
        };
    }

    [Theory]
    [MemberData(nameof(Handle_ShouldThrow_WhenColorAlreadyExists_TestCases))]
    public async Task Handle_ShouldThrow_WhenColorAlreadyExists(AddColorCommand command)
    {
        // Arrange
        var sut = new AddColorCommandHandler(_fixture.ColorRepository);
        var ct = new CancellationToken();
    
        _ = sut.Handle(command, ct);
        await _fixture.FerdinandDbContext.SaveChangesAsync(ct);
    
        // Act
        var action = async () =>
        {
            await sut.Handle(command, ct);
            await _fixture.FerdinandDbContext.SaveChangesAsync(ct);
        };
    
        // Assert
        await action.Should().ThrowAsync<Exception>();
    }
    
    public static IEnumerable<object[]> Handle_ShouldThrow_WhenColorAlreadyExists_TestCases()
    {
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Blue) };
    }
}
