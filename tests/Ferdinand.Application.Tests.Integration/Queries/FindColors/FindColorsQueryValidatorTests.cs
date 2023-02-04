using Ferdinand.Application.Queries.FindColors;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Queries.FindColors;

public class FindColorsQueryValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("F")]
    [InlineData("FF")]
    [InlineData("FFF")]
    [InlineData("FFFF")]
    [InlineData("FFFFF")]
    [InlineData("FFFFFF")]
    [InlineData("123456")]
    public void Validate_ShouldBeValid_WhenInvokedWithValidInput(string hexvalue)
    {
        // Arrange
        var query = new FindColorsQuery(hexvalue);
        var sut = new FindColorsQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("FFFFFFF")]
    [InlineData("1234567")]
    [InlineData("Z")]
    public void Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput(string hexValue)
    {
        // Arrange
        var query = new FindColorsQuery(hexValue);
        var sut = new FindColorsQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
