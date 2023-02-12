using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Ferdinand.Data.EntityFrameworkCore;
using Ferdinand.Data.EntityFrameworkCore.Repositories;
using Ferdinand.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Ferdinand.Data.Tests.Integration.EntityFrameworkCore;

public class HostFixture : IAsyncLifetime
{
    public IHost Host { get; private set; } = null!;

    private readonly TestcontainerDatabase _dbContainer;

    public HostFixture()
    {
        _dbContainer = new ContainerBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "ferdinand_data_test",
                Username = "ferdinand",
                Password = "ferdinand"
            })
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
                    opts.UseNpgsql(_dbContainer.ConnectionString));
                services.AddTransient<IColorRepository, ColorRepository>();
            })
            .Build();
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
