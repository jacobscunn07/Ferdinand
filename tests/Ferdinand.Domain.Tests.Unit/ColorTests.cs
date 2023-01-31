using Ferdinand.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Domain.Tests.Unit;

public class ColorTests
{
    [Fact]
    public void FromHexValue_ShouldCreateColor_WhenPassedValidInputs()
    {
        // Arrange
        var tenant = Tenant.Create("tenant 1");
        var hexValue = "FFFFFF";
        var description = "white";

        // Act
        var sut = Color.FromHexValue(tenant, hexValue, description);

        // Assert
        sut.Tenant.Should().Be(tenant);
        sut.HexValue.Should().Be(hexValue);
        sut.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("tenant1", "", "")]
    [InlineData("", "123456", "")]
    [InlineData("tenant1", "1234567", "")]
    [InlineData("tenant1", "ZZZZZZ", "this color does not exist")]
    public void FromHexValue_ShouldThrowDomainException_WhenPassedInvalidInputs(
        string tenant,
        string hexValue,
        string description
    )
    {
        // Arrange
        var sut = () => Color.FromHexValue(Tenant.Create(tenant), hexValue, description);

        // Act
        // Assert
        sut.Should().Throw<DomainException>();
    }
}
