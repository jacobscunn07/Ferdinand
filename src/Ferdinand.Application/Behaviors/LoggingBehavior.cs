using MediatR;
using Microsoft.Extensions.Logging;

namespace Ferdinand.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();

            _logger.LogInformation("APP: {RequestType} => OK", typeof(TRequest).Name);

            return response;
        }
        catch
        {
            throw;
        }
    }
}
