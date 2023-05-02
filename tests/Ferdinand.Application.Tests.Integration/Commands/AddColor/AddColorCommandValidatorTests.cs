using Ferdinand.Application.Commands.AddColor;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Commands.AddColor;

public class AddColorCommandValidatorTests
{
    [Theory]
    [InlineData("Tenant", "000000", "")]
    [InlineData("Tenant", "000000", "Description...")]
    [InlineData("Tenant", "000000", null)]
    public void Validate_ShouldBeValid_WhenInvokedWithValidInput(
        string tenant,
        string hexValue,
        string description)
    {
        // Arrange
        var command = new AddColorCommand(tenant, hexValue, description);
        var sut = new AddColorCommandValidator();

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("", "", "")]
    [InlineData("Tenant", "", "")]
    [InlineData("", "000000", "")]
    [InlineData("Tenant", "0", "")]
    [InlineData("Tenant", "00", "")]
    [InlineData("Tenant", "000", "")]
    [InlineData("Tenant", "0000", "")]
    [InlineData("Tenant", "00000", "")]
    [InlineData("Tenant", "0000000", "")]
    [InlineData("Tenant", "GGGGGG", "")]
    public void Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput(
        string tenant,
        string hexValue,
        string description)
    {
        // Arrange
        var command = new AddColorCommand(tenant, hexValue, description);
        var sut = new AddColorCommandValidator();

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
