using FluentAssertions;
using Xunit;

namespace Ferdinand.Domain.Tests.Unit;

public class TenantTests
{
    [Fact]
    public void Create_ShouldCreate_WhenValidInput()
    {
        // Arrange
        var tenant = "A";
        Tenant sut;

        // Act
        sut = Tenant.Create(tenant);

        // Assert
        sut.Value.Should().Be(tenant);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldNotCreate_WhenInvalidInput(string tenant)
    {
        // Arrange
        Tenant sut;

        // Act
        var action = () => sut = Tenant.Create(tenant);

        // Assert
        action.Should().Throw<DomainException>();
    }
}
