using Ferdinand.Domain.Repositories;
using MediatR;

namespace Ferdinand.Application.Queries.FindColors;

public class FindColorsQueryHandler : IRequestHandler<FindColorsQuery, FindColorsQueryResult>
{
    private readonly IColorRepository _repository;

    public FindColorsQueryHandler(IColorRepository repository)
    {
        _repository = repository;
    }
    
    public Task<FindColorsQueryResult> Handle(FindColorsQuery request, CancellationToken cancellationToken)
    {
        var colors = _repository
            .Find(x => x.HexValue.Contains(request.SearchTerm))
            .Select(x => new FindColorsQueryResultColor(x.Key.Value, x.HexValue, x.Description));


        return Task.FromResult(new FindColorsQueryResult(colors));
    }
}
