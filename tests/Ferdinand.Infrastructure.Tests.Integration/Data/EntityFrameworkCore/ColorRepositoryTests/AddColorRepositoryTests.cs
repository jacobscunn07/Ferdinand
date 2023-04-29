using Bogus;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Throw;
using Xunit;

namespace Ferdinand.Infrastructure.Tests.Integration.Data.EntityFrameworkCore.ColorRepositoryTests;

[Collection("EntityFrameworkCore ColorRepository Collection")]
public class AddColorRepositoryTests
{
    private readonly HostFixture _fixture;

    public AddColorRepositoryTests(HostFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Add_ShouldAddColorToDatabase_WhenInvoked()
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

        // Assert
        (await sut.Exists(color.Key)).Should().BeTrue();
    }
    
    [Fact]
    public async Task Add_ShouldAddColorToDatabase_WhenHexValuesAreTheSameButDifferentTenants()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var sut = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var hexValue = new Randomizer().Hexadecimal(6, "");
        
        var tenant1 = Tenant.Create(new Faker().Company.CompanyName());
        var color1 = Color.FromHexValue(tenant1, hexValue);

        var tenant2 = Tenant.Create(new Faker().Company.CompanyName());
        var color2 = Color.FromHexValue(tenant2, hexValue);
        
        // Act
        await sut.Add(color1);
        await sut.Add(color2);
        await ctx.SaveChangesAsync();

        // Assert
        (await sut.Exists(color1.Key)).Should().BeTrue();
        (await sut.Exists(color2.Key)).Should().BeTrue();
    }

    [Fact]
    public async Task Add_ShouldThrow_WhenAddingDuplicateTenantAndHexValueCombination()
    {
        // Arrange
        using var scope = _fixture.Host.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();
        var colorRepository = scope.ServiceProvider.GetRequiredService<IColorRepository>();

        var tenant = Tenant.Create(new Faker().Company.CompanyName());
        var hexValue = new Randomizer().Hexadecimal(6, "");
        var color = Color.FromHexValue(tenant, hexValue);

        // Act
        await colorRepository.Add(color);
        await ctx.SaveChangesAsync();
        
        await colorRepository.Add(color);
        var sut = async () => await ctx.SaveChangesAsync();

        // Assert
        sut.Should().Throw();
    }
}
