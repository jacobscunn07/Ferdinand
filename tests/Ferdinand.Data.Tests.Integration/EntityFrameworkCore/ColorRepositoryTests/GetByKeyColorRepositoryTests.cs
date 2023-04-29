using Ferdinand.Infrastructure.EntityFrameworkCore;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Throw;
using Xunit;

namespace Ferdinand.Data.Tests.Integration.EntityFrameworkCore.ColorRepositoryTests;

[Collection("EntityFrameworkCore ColorRepository Collection")]
public class GetByKeyColorRepositoryTests
{
    private readonly HostFixture _fixture;

    public GetByKeyColorRepositoryTests(HostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByKey_ShouldReturnColor_WhenKeyExists()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var tenant = Tenant.Create("tenant");
        var hexValue = new Bogus.Randomizer().Hexadecimal(6, "");
        var color = Color.FromHexValue(tenant, hexValue);

        await sut.Add(color);
        await ctx.SaveChangesAsync();

        // Act
        var colorFromDb = await sut.GetByKey(color.Key);

        // Assert
        colorFromDb.Should().NotBeNull();
        colorFromDb.Should().BeEquivalentTo(color);
    }
    
    [Fact]
    public Task GetByKey_ShouldNotReturnColor_WhenNoKeyExists()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        var key = ColorKey.CreateUnique();

        // Act
        var action = async () => await sut.GetByKey(key);

        // Assert
        action.Should().Throw();
        return Task.CompletedTask;
    }
}
