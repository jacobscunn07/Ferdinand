using MediatR;

namespace Ferdinand.Application.Queries.FindColors;

public record FindColorsQuery(string SearchTerm) : IRequest<FindColorsQueryResult>;
