using Ferdinand.Application.Queries.GetColor;

namespace Ferdinand.Testing.Queries;

public static class GetColorQueryBuilder
{
    public static GetColorQuery CreateQuery(Guid colorKey) => new(colorKey);
}
