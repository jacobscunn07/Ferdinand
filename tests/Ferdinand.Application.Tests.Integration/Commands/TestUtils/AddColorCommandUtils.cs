using Ferdinand.Application.Commands.AddColor;
using Ferdinand.Application.Tests.Integration.TestUtils.Constants;

namespace Ferdinand.Application.Tests.Integration.Commands.TestUtils;

public static class AddColorCommandUtils
{
    public static AddColorCommand CreateCommand(
        string? tenant = null,
        string? hexValue = null,
        string? description = null) =>
        new(
            tenant ?? Constants.Tenant.Name,
            hexValue ?? Constants.Color.HexValue,
            description ?? Constants.Color.Description);
}