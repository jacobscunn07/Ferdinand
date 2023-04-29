using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ferdinand.Infrastructure.Tests.Integration.Data.EntityFrameworkCore.ColorRepositoryTests;

[Collection("EntityFrameworkCore ColorRepository Collection")]
public class FindColorRepositoryTests
{
    private readonly HostFixture _fixture;

    public FindColorRepositoryTests(HostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Find_ShouldReturnMatchingColors_WhenInvoked()
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
        var colorsFromDb = sut.Find(x => x.HexValue == hexValue);
        var colorFromDb = colorsFromDb.FirstOrDefault();

        // Assert
        colorsFromDb.Should().HaveCount(1);
        colorFromDb.Should().NotBeNull();
        colorFromDb.Should().BeEquivalentTo(color);
    }
}
