using MediatR;

namespace Ferdinand.Application.Commands.AddColor;

public record AddColorCommand(string Tenant, string HexValue, string Description) : IRequest;
