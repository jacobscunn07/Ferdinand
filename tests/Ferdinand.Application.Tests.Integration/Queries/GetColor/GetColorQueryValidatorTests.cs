using Ferdinand.Application.Queries.GetColor;
using Ferdinand.Testing.Builders;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Queries.GetColor;

public class GetColorQueryValidatorTests
{
    [Theory]
    [MemberData(nameof(Validate_ShouldBeValid_WhenInvokedWithValidInput_TestCases))]
    public void Validate_ShouldBeValid_WhenInvokedWithValidInput(GetColorQuery query)
    {
        // Arrange
        var sut = new GetColorQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> Validate_ShouldBeValid_WhenInvokedWithValidInput_TestCases()
    {
        yield return new object[] { GetColorQueryBuilder.CreateQuery("00000000-0000-0000-0000-000000000001") };
    }
    
    [Theory]
    [MemberData(nameof(Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput_TestCases))]
    public void Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput(GetColorQuery query)
    {
        // Arrange
        var sut = new GetColorQueryValidator();

        // Act
        var result = sut.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput_TestCases()
    {
        yield return new object[] { GetColorQueryBuilder.CreateQuery("00000000-0000-0000-0000-000000000000") };
    }
}
