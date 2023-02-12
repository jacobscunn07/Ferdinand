using Ferdinand.Application.Queries.GetColor;
using Ferdinand.Data.EntityFrameworkCore;
using Ferdinand.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Throw;
using Xunit;
using Color = Ferdinand.Domain.Models.Color;

namespace Ferdinand.Application.Tests.Integration.Queries.GetColor;

public class GetColorQueryHandlerTests : IClassFixture<HostFixture>
{
    private readonly HostFixture _fixture;

    public GetColorQueryHandlerTests(HostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_ShouldGetColor_WhenColorExistsWithKey()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
        var color = Color.FromHexValue(tenant, hexValue);

        await repository.Add(color);
        await ctx.SaveChangesAsync();

        var query = new GetColorQuery(color.Key.Value);
        var sut = new GetColorQueryHandler(repository);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        result.Should().NotBeNull();
        result.Color.Key.Should().Be(color.Key.Value);
        result.Color.HexValue.Should().Be(color.HexValue);
        result.Color.Description.Should().BeEquivalentTo(color.Description);
    }
    
    [Fact]
    public Task Handle_ShouldThrow_WhenColorDoesNotExistWithKey()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        var query = new GetColorQuery(Guid.NewGuid());
        var sut = new GetColorQueryHandler(repository);

        // Act
        var action = async () => await sut.Handle(query, new CancellationToken());

        // Assert
        action.Should().Throw();
        
        return Task.CompletedTask;
    }
}
