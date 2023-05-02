using Ferdinand.Application.Commands.AddColor;
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
    [InlineData("t", "000000", "")]
    [InlineData("t", "FFFFFF", "white")]
    public async Task Handle_ShouldAddColor_WhenInvokedWithValidInputs(string tenant, string hexValue, string description)
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var tenantObject = Tenant.Create(tenant);
        var command = new AddColorCommand(tenantObject.Value, hexValue, description);
        var sut = new AddColorCommandHandler(repository);

        // Act
        var result = await sut.Handle(command, new CancellationToken());
        await ctx.SaveChangesAsync();

        // Assert
        (await repository.Exists(ColorKey.Create(result.Key))).Should().BeTrue();
        (await repository.GetByKey(ColorKey.Create(result.Key)))
            .Should()
            .BeEquivalentTo(
                Color.FromHexValue(tenantObject, hexValue, description),
                config =>
                    config
                        .Excluding(c => c.Key)
                        .ComparingByMembers<Color>()
            );
    }
    
    [Fact]
    public void Handle_ShouldThrow_WhenInvokedWithInvalidInputs()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var tenant = "t";
        var hexValue = "FFFFFFF";
        var description = "white";
        var tenantObject = Tenant.Create(tenant);
        var command = new AddColorCommand(tenantObject.Value, hexValue, description);
        var handler = new AddColorCommandHandler(repository);

        // Act
        var action = async () => await handler.Handle(command, new CancellationToken());

        // Assert
        action.Should().ThrowAsync<DomainException>();
    }
    
    [Fact]
    public async Task Handle_ShouldThrow_WhenColorAlreadyExists()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();
    
        var tenant = "t";
        var hexValue = "111111";
        var description = "white";
        var tenantObject = Tenant.Create(tenant);
        var command = new AddColorCommand(tenantObject.Value, hexValue, description);
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
}
