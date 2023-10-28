using Ferdinand.Application.Queries.GetColor;
using Ferdinand.Testing;
using Ferdinand.Testing.Builders;
using FluentAssertions;
using Throw;
using Xunit;
using Color = Ferdinand.Domain.Models.Color;
using Tenant = Ferdinand.Domain.Models.Tenant;

namespace Ferdinand.Application.Tests.Integration.Queries.GetColor;

public class GetColorQueryHandlerTests : IClassFixture<HostFixture>
{
    private readonly HostFixture _fixture;

    public GetColorQueryHandlerTests(HostFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [MemberData(nameof(Handle_ShouldGetColor_WhenColorExistsWithKey_TestCases))]
    public async Task Handle_ShouldGetColor_WhenColorExistsWithKey(Tenant tenant, string hexValue)
    {
        // Arrange
        var color = Color.FromHexValue(tenant, hexValue);
        await _fixture.ColorRepository.Add(color);
        await _fixture.FerdinandDbContext.SaveChangesAsync();
        var query = GetColorQueryBuilder.CreateQuery(color.Key.Value);
        var sut = new GetColorQueryHandler(_fixture.ColorRepository);

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        result.Should().NotBeNull();
        result.Color.Key.Should().Be(color.Key.Value);
        result.Color.HexValue.Should().Be(color.HexValue);
        result.Color.Description.Should().BeEquivalentTo(color.Description);
    }

    public static IEnumerable<object[]> Handle_ShouldGetColor_WhenColorExistsWithKey_TestCases()
    {
        yield return new object[] { Constants.Tenant.Name, Constants.Color.HexValue.Black };
    }
    
    [Theory]
    [MemberData(nameof(Handle_ShouldThrow_WhenColorDoesNotExistWithKey_TestCases))]
    public Task Handle_ShouldThrow_WhenColorDoesNotExistWithKey(GetColorQuery query)
    {
        // Arrange
        var sut = new GetColorQueryHandler(_fixture.ColorRepository);

        // Act
        var action = async () => await sut.Handle(query, new CancellationToken());

        // Assert
        action.Should().Throw();
        
        return Task.CompletedTask;
    }
    
    public static IEnumerable<object[]> Handle_ShouldThrow_WhenColorDoesNotExistWithKey_TestCases()
    {
        yield return new object[] { GetColorQueryBuilder.CreateQuery(Guid.NewGuid()) };
    }
}
