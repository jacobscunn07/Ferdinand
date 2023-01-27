using Ferdinand.Application.Commands.MigrateDatabase;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class MigrationRunnerService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IMediator _mediator;
    private readonly ILogger<MigrationRunnerService> _logger;

    public MigrationRunnerService(
        IHostApplicationLifetime hostApplicationLifetime,
        IMediator mediator,
        ILogger<MigrationRunnerService> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _mediator.Send(new MigrateDatabaseCommand(), cancellationToken);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An unknown error occurred while executing migrations");
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
