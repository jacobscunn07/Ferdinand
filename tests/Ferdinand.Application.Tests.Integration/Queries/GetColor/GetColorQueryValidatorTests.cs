using Ferdinand.Application.Queries.GetColor;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Queries.GetColor;

public class GetColorQueryValidatorTests
{
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    public void Validate_ShouldBeValid_WhenInvokedWithValidInput(Guid key)
    {
        // Arrange
        var query = new GetColorQuery(key);
        var sut = new GetColorQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput(Guid key)
    {
        // Arrange
        var query = new GetColorQuery(key);
        var sut = new GetColorQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
