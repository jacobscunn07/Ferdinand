using Ferdinand.Application.Queries.GetColor;

namespace Ferdinand.Testing.Builders;

public static class GetColorQueryBuilder
{
    public static GetColorQuery CreateQuery(Guid colorKey) => new(colorKey);
    public static GetColorQuery CreateQuery(string colorKey) => new(Guid.Parse(colorKey));
}
