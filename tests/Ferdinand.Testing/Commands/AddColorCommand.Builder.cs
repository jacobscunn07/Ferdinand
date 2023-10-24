using Ferdinand.Application.Commands.AddColor;

namespace Ferdinand.Testing.Commands;

public static class AddColorCommandBuilder
{
    public static AddColorCommand CreateCommand(
        string? tenant = null,
        string? hexValue = null,
        string? description = null) =>
        new(
            tenant ?? Constants.Tenant.Name,
            hexValue ?? Constants.Color.HexValue.Black,
            description ?? Constants.Color.Description);
}
