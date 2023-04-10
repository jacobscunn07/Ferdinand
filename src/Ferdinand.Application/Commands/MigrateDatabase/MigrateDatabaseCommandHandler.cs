using Ferdinand.Data.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ferdinand.Application.Commands.MigrateDatabase;

public sealed class MigrateDatabaseCommandHandler : IRequestHandler<MigrateDatabaseCommand>
{
    private readonly ILogger<MigrateDatabaseCommandHandler> _logger;
    private readonly FerdinandDbContext _dbContext;

    public MigrateDatabaseCommandHandler(ILogger<MigrateDatabaseCommandHandler> logger, FerdinandDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(MigrateDatabaseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing migrations...");
        if (await _dbContext.Database.EnsureCreatedAsync(cancellationToken))
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }
        _logger.LogInformation("Executing migrations completed...");
        
        return await Task.FromResult(Unit.Value);
    }
}
