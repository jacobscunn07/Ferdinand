using Ferdinand.Infrastructure.EntityFrameworkCore;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ferdinand.Data.Tests.Integration.EntityFrameworkCore.ColorRepositoryTests;

[Collection("EntityFrameworkCore ColorRepository Collection")]
public class RemoveColorRepositoryTests
{
    private readonly HostFixture _fixture;

    public RemoveColorRepositoryTests(HostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Remove_ShouldRemoveColorFromDatabase_WhenInvoked()
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
        await sut.Remove(colorFromDb);
        await ctx.SaveChangesAsync();

        // Assert
        (await sut.Exists(color.Key)).Should().BeFalse();
    }
}
