using Ferdinand.Application.Commands.AddColor;
using Ferdinand.Application.Tests.Integration.Commands.TestUtils;
using Ferdinand.Application.Tests.Integration.TestUtils.Colors.Extensions;
using Ferdinand.Domain;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        var sut = new AddColorCommandHandler(repository);

        // Act
        var result = await sut.Handle(command, new CancellationToken());
        await ctx.SaveChangesAsync();

        // Assert
        (await repository.Exists(ColorKey.Create(result.Key))).Should().BeTrue();
        (await repository.GetByKey(ColorKey.Create(result.Key))).ValidateCreatedFrom(command);
    }

    public static IEnumerable<object[]> Handle_ShouldAddColor_WhenInvokedWithValidInputs_TestCases()
    {
        yield return new[] { AddColorCommandUtils.CreateCommand() };
        yield return new[] { AddColorCommandUtils.CreateCommand(tenant: "ABC", description: "") };
    }

    [Theory]
    [MemberData(nameof(Handle_ShouldThrow_WhenInvokedWithInvalidInputs_TestCases))]
    public void Handle_ShouldThrow_WhenInvokedWithInvalidInputs(AddColorCommand command)
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        var sut = new AddColorCommandHandler(repository);

        // Act
        var action = async () => await sut.Handle(command, new CancellationToken());

        // Assert
        action.Should().ThrowAsync<DomainException>();
    }

    public static IEnumerable<object[]> Handle_ShouldThrow_WhenInvokedWithInvalidInputs_TestCases()
    {
        yield return new[] { AddColorCommandUtils.CreateCommand(hexValue: "FFFFFFF") };
    }

    [Theory]
    [MemberData(nameof(Handle_ShouldThrow_WhenColorAlreadyExists_TestCases))]
    public async Task Handle_ShouldThrow_WhenColorAlreadyExists(AddColorCommand command)
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        var sut = new AddColorCommandHandler(repository);
        var ct = new CancellationToken();
    
        _ = sut.Handle(command, ct);
        await ctx.SaveChangesAsync(ct);
    
        // Act
        var action = async () =>
        {
            await sut.Handle(command, ct);
            await ctx.SaveChangesAsync(ct);
        };
    
        // Assert
        await action.Should().ThrowAsync<Exception>();
    }
    
    public static IEnumerable<object[]> Handle_ShouldThrow_WhenColorAlreadyExists_TestCases()
    {
        yield return new[] { AddColorCommandUtils.CreateCommand(hexValue: "111111") };
    }
}
