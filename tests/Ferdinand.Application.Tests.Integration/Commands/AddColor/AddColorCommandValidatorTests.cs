using Ferdinand.Application.Commands.AddColor;
using Ferdinand.Testing;
using Ferdinand.Testing.Builders;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Application.Tests.Integration.Commands.AddColor;

public class AddColorCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(Validate_ShouldBeValid_WhenInvokedWithValidInput_TestCases))]
    public void Validate_ShouldBeValid_WhenInvokedWithValidInput(AddColorCommand command)
    {
        // Arrange
        var sut = new AddColorCommandValidator();

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> Validate_ShouldBeValid_WhenInvokedWithValidInput_TestCases()
    {
        yield return new object[] { AddColorCommandBuilder.CreateCommand() };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(description: "") };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(description: null) };
    }
    
    [Theory]
    [MemberData(nameof(Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput_TestCases))]
    public void Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput(AddColorCommand command)
    {
        // Arrange
        var sut = new AddColorCommandValidator();

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> Validate_ShouldBeInvalid_WhenInvokedWithInvalidInput_TestCases()
    {
        yield return new object[] { AddColorCommandBuilder.CreateCommand(tenant: "", hexValue: "", description: "") };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: "") };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: "") };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Pink[..1]) };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Pink[..2]) };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Pink[..3]) };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Pink[..4]) };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Pink[..5]) };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Pink + Constants.Color.HexValue.Pink) };
        yield return new object[] { AddColorCommandBuilder.CreateCommand(hexValue: Constants.Color.HexValue.Invalid) };
    }
}
