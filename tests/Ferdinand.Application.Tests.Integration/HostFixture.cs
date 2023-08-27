using Ferdinand.Domain.Repositories;
using Ferdinand.Infrastructure.Data.EntityFrameworkCore.Repositories;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Xunit;

namespace Ferdinand.Application.Tests.Integration;

public class HostFixture : IAsyncLifetime
{
    public IHost Host { get; private set; } = null!;

    private IServiceScope _scope;

    private readonly PostgreSqlContainer _dbContainer;

    public FerdinandDbContext FerdinandDbContext => _scope.ServiceProvider.GetRequiredService<FerdinandDbContext>();

    public IColorRepository ColorRepository => _scope.ServiceProvider.GetRequiredService<IColorRepository>();

    public HostFixture()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithDatabase("ferdinand")
            .WithUsername("ferdinand")
            .WithPassword("ferdinand")
            .WithImage("postgres:15.1")
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await StartContainers();
        BuildHost();
        await ExecuteDatabaseMigrations();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private async Task StartContainers()
    {
        await _dbContainer.StartAsync();
    }

    private void BuildHost()
    {
        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<FerdinandDbContext>(opts =>
                    opts.UseNpgsql(_dbContainer.GetConnectionString()));
                services.AddTransient<IColorRepository, ColorRepository>();
                services.AddTransient<OutboxMessageRepository>();
            })
            .Build();

        _scope = Host.Services.CreateScope();
    }

    private async Task ExecuteDatabaseMigrations()
    {
        await Host
            .Services
            .GetRequiredService<FerdinandDbContext>()
            .Database
            .MigrateAsync();
    }
}