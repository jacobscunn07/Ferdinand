using Ferdinand.Data.EntityFrameworkCore;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ferdinand.Data.Tests.Integration.EntityFrameworkCore.ColorRepositoryTests;

[Collection("EntityFrameworkCore ColorRepository Collection")]
public class RemoveRangeColorRepositoryTests
{
    private readonly HostFixture _fixture;

    public RemoveRangeColorRepositoryTests(HostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task RemoveRange_ShouldRemoveColorsFromDatabase_WhenInvoked()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = new Bogus.Randomizer().Hexadecimal(6, "");
        var color = Color.FromHexValue(tenant, hexValue);
        var colors = new List<Color> { color };
        await sut.AddRange(colors);
        await ctx.SaveChangesAsync();

        // Act
        var colorFromDb = await sut.GetByKey(color.Key);
        await sut.RemoveRange(new List<Color> { colorFromDb });
        await ctx.SaveChangesAsync();

        // Assert
        (await sut.Exists(color.Key)).Should().BeFalse();
    }
}
