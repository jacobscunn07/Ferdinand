using Ferdinand.Application.Queries.GetColor;

namespace Ferdinand.Application.Tests.Integration.Queries.TestUtils;

public static class GetColorQueryUtils
{
    public static GetColorQuery CreateCommand(Guid colorKey) => new(colorKey);
}
