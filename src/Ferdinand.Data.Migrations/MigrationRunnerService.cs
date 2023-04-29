using Ferdinand.Infrastructure.EntityFrameworkCore;
using Ferdinand.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

public class MigrationRunnerService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly FerdinandDbContext _ctx;
    private readonly ILoggerAdapter<MigrationRunnerService> _logger;

    public MigrationRunnerService(
        IHostApplicationLifetime hostApplicationLifetime,
        FerdinandDbContext ctx,
        ILoggerAdapter<MigrationRunnerService> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _ctx = ctx;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Executing migrations...");
                if (await _ctx.Database.EnsureCreatedAsync(cancellationToken))
                {
                    await _ctx.Database.MigrateAsync(cancellationToken);
                }
                _logger.LogInformation("Executing migrations completed...");
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
