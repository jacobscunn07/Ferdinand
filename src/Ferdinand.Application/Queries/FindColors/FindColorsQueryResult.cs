namespace Ferdinand.Application.Queries.FindColors;

public record FindColorsQueryResult(IEnumerable<FindColorsQueryResultColor> Colors);

public record FindColorsQueryResultColor(Guid Key, string HexValue, string Description);
