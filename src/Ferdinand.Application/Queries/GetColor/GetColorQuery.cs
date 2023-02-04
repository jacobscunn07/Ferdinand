using MediatR;

namespace Ferdinand.Application.Queries.GetColor;

public record GetColorQuery(Guid Key) : IRequest<GetColorQueryResult>;
