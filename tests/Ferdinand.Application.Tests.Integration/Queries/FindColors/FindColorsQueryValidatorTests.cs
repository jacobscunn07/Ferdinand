using Ferdinand.Application.Queries.FindColors;
using Ferdinand.Testing;
using Ferdinand.Testing.Builders;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Queries.FindColors;

public class FindColorsQueryValidatorTests
{
    [Theory]
    [MemberData(nameof(Validate_ShouldBeValid_WhenInvokedWithValidInput_TestCases))]
    public void Validate_ShouldBeValid_WhenInvokedWithValidInput(FindColorsQuery query)
    {
        // Arrange
        var sut = new FindColorsQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> Validate_ShouldBeValid_WhenInvokedWithValidInput_TestCases()
    {
        yield return new object[] { FindColorsQueryBuilder.CreateQuery() };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Orange[..1]) };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Orange[..2]) };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Orange[..3]) };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Orange[..4]) };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Orange[..5]) };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Orange[..6]) };
    }
    
    [Theory]
    [MemberData(nameof(Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput_TestCases))]
    public void Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput(FindColorsQuery query)
    {
        // Arrange
        var sut = new FindColorsQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput_TestCases()
    {
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Yellow + Constants.Color.HexValue.Yellow) };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery(Constants.Color.HexValue.Yellow + "A") };
        yield return new object[] { FindColorsQueryBuilder.CreateQuery("Z") };
    }
}
