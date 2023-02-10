using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using MediatR;

namespace Ferdinand.Application.Commands.AddColor;

public class AddColorCommandHandler : IRequestHandler<AddColorCommand>
{
    private readonly IColorRepository _repository;

    public AddColorCommandHandler(IColorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(AddColorCommand request, CancellationToken cancellationToken)
    {
        var tenant = Tenant.Create(request.Tenant);
        var color = Color.FromHexValue(tenant, request.HexValue, request.Description);

        await _repository.Add(color);

        return Unit.Value;
    }
}
