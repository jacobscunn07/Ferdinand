using Ferdinand.Application.Queries.FindColors;

namespace Ferdinand.Testing.Builders;

public static class FindColorsQueryBuilder
{
    public static FindColorsQuery CreateQuery(string? searchTerm = null) => new(searchTerm ?? "");
}
