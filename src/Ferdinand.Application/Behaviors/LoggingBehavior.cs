using MediatR;
using Microsoft.Extensions.Logging;

namespace Ferdinand.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();

            _logger.LogDebug("APP: {RequestType} => OK", typeof(TRequest).Name);

            return response;
        }
        catch(Exception e)
        {
            _logger.LogError(
                e,
                "APP: {RequestType} => ERROR: {ErrorMessage}",
                typeof(TRequest).Name,
                e.Message);

            throw;
        }
    }
}
