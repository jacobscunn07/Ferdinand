using Ferdinand.Common.Logging;
using MediatR;

namespace Ferdinand.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILoggerAdapter<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILoggerAdapter<LoggingBehavior<TRequest, TResponse>> logger)
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
