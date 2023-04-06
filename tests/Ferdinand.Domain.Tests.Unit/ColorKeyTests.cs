using Ferdinand.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Ferdinand.Domain.Tests.Unit;

public class ColorKeyTests
{
    [Fact]
    public void Create_ShouldCreateColorKeyFromString_WhenPassedValidGuidAsString()
    {
        // Arrange
        var guid = "00000000-0000-0000-0000-000000000001";
        ColorKey sut;

        // Act
        sut = ColorKey.Create(guid);

        // Assert
        sut.Value.ToString().Should().Be(guid);
    }
    
    [Fact]
    public void Create_ShouldThrow_WhenPassedInvalidGuidAsString()
    {
        // Arrange
        var guid = "a";
        var sut = () => ColorKey.Create(guid);

        // Act
        // Assert
        sut.Should().Throw<DomainException>();
    }
    
    [Fact]
    public void Create_ShouldCreateColorKeyFromGuid_WhenPassedValidGuid()
    {
        // Arrange
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000001");
        ColorKey sut;

        // Act
        sut = ColorKey.Create(guid);

        // Assert
        sut.Value.Should().Be(guid);
    }
}
