using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using MediatR;

namespace Ferdinand.Application.Queries.GetColor;

public class GetColorQueryHandler : IRequestHandler<GetColorQuery, GetColorQueryResult>
{
    private readonly IColorRepository _colors;

    public GetColorQueryHandler(IColorRepository colors)
    {
        _colors = colors;
    }
    
    public async Task<GetColorQueryResult> Handle(GetColorQuery request, CancellationToken cancellationToken)
    {
        var color = await _colors.GetByKey(ColorId.Create(request.Key));
        var colorResult = new Color(color.Key.Value, color.HexValue, color.Description);

        return new GetColorQueryResult(colorResult);
    }
}
