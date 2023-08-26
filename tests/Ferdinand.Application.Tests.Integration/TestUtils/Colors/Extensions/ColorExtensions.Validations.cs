using Ferdinand.Application.Commands.AddColor;
using Ferdinand.Domain.Models;
using FluentAssertions;

namespace Ferdinand.Application.Tests.Integration.TestUtils.Colors.Extensions;

public static partial class ColorExtensions
{
    public static void ValidateCreatedFrom(this Color color, AddColorCommand command)
    {
        color.Tenant.Value.Should().Be(command.Tenant);
        color.HexValue.Should().Be(command.HexValue);
        color.Description.Should().Be(command.Description);
    }
}
