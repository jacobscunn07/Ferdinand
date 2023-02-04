namespace Ferdinand.Application.Queries.GetColor;

public record GetColorQueryResult(Color Color);
public record Color(Guid Key, string HexValue, string Description);
