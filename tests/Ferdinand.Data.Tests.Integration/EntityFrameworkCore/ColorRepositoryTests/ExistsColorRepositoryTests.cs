using Ferdinand.Data.EntityFrameworkCore;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ferdinand.Data.Tests.Integration.EntityFrameworkCore.ColorRepositoryTests;

[Collection("EntityFrameworkCore ColorRepository Collection")]
public class ExistsColorRepositoryTests
{
    private readonly HostFixture _fixture;

    public ExistsColorRepositoryTests(HostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Exists_ShouldBeTrue_WhenColorDoesExist()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = new Bogus.Randomizer().Hexadecimal(6, "");
        var color = Color.FromHexValue(tenant, hexValue);

        // Act
        await sut.Add(color);
        await ctx.SaveChangesAsync();
        var exists = await sut.Exists(color.Key);

        // Assert
        exists.Should().BeTrue();
    }
    
    [Fact]
    public async Task Exists_ShouldBeFalse_WhenColorDoesNotExist()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var colorKey = ColorKey.CreateUnique();

        // Act
        var exists = await sut.Exists(colorKey);

        // Assert
        exists.Should().BeFalse();
    }
}
