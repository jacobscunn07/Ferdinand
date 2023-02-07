using Ferdinand.Data.EntityFrameworkCore;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Throw;
using Xunit;

namespace Ferdinand.Data.Tests.Integration.EntityFrameworkCore;

public class ColorRepositoryTests : BaseTest
{
    [Fact]
    public async Task GetByKey_ShouldReturnColor_WhenKeyExists()
    {
        // Arrange
        using var scope = _host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
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
        using var scope = _host.Services.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        var key = ColorId.CreateUnique();

        // Act
        var action = async () => await sut.GetByKey(key);

        // Assert
        action.Should().Throw();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Add_ShouldAddColorToDatabase_WhenInvoked()
    {
        // Arrange
        using var scope = _host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
        var color = Color.FromHexValue(tenant, hexValue);

        // Act
        await sut.Add(color);
        await ctx.SaveChangesAsync();

        // Assert
        (await sut.Exists(color.Key)).Should().BeTrue();
    }
    
    [Fact]
    public async Task AddRange_ShouldAddColorsToDatabase_WhenInvoked()
    {
        // Arrange
        using var scope = _host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
        var color = Color.FromHexValue(tenant, hexValue);
        var colors = new List<Color> { color };

        // Act
        await sut.AddRange(colors);
        await ctx.SaveChangesAsync();

        // Assert
        (await sut.Exists(color.Key)).Should().BeTrue();
    }
    
    [Fact]
    public async Task Exists_ShouldBeTrue_WhenColorDoesExist()
    {
        // Arrange
        using var scope = _host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
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
        using var scope = _host.Services.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var colorKey = ColorId.CreateUnique();

        // Act
        var exists = await sut.Exists(colorKey);

        // Assert
        exists.Should().BeFalse();
    }
    
    [Fact]
    public async Task Remove_ShouldRemoveColorFromDatabase_WhenInvoked()
    {
        // Arrange
        using var scope = _host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
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
    
    [Fact]
    public async Task RemoveRange_ShouldRemoveColorsFromDatabase_WhenInvoked()
    {
        // Arrange
        using var scope = _host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
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

    [Fact]
    public async Task Find_ShouldReturnMatchingColors_WhenInvoked()
    {
        // Arrange
        using var scope = _host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();
        
        var tenant = Tenant.Create("tenant");
        var hexValue = "000000";
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
