using Microsoft.Extensions.Logging;
using Quartz;

namespace Ferdinand.Jobs;

public sealed class PublishOutboxMessageJob : IJob
{
    private readonly ILogger<PublishOutboxMessageJob> _logger;

    public PublishOutboxMessageJob(ILogger<PublishOutboxMessageJob> logger)
    {
        _logger = logger;
    }
    
    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Executing {Name}", GetType().Name);
        return Task.CompletedTask;
    }
}
