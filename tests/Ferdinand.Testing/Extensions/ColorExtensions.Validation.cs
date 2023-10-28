using Ferdinand.Application.Commands.AddColor;
using Ferdinand.Domain.Models;
using FluentAssertions;

namespace Ferdinand.Testing.Extensions;

public static class ColorExtensions
{
    public static void ValidateCreatedFrom(this Color color, AddColorCommand command)
    {
        color.Tenant.Value.Should().Be(command.Tenant);
        color.HexValue.Should().Be(command.HexValue);
        color.Description.Should().Be(command.Description);
    }
}
