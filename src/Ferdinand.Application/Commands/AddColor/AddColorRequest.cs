using MediatR;

namespace Ferdinand.Application.Commands.AddColor;

public record AddColorRequest(string HexValue, string Description) : IRequest;
