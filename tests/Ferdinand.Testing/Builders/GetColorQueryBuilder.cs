using Ferdinand.Application.Queries.GetColor;

namespace Ferdinand.Testing.Builders;

public static class GetColorQueryBuilder
{
    public static GetColorQuery CreateQuery(Guid colorKey) => new(colorKey);
}
