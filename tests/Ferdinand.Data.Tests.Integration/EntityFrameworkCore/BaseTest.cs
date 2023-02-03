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

public abstract class BaseTest: IAsyncLifetime
{
    private readonly TestcontainerDatabase _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "ferdinand_data_test",
            Username = "ferdinand",
            Password = "ferdinand"
        })
        .Build();

    protected IHost _host;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<FerdinandDbContext>(opts => opts.UseNpgsql(_dbContainer.ConnectionString));
                services.AddTransient<IColorRepository, ColorRepository>();
            })
            .Build();

        await _host
            .Services
            .GetRequiredService<FerdinandDbContext>()
            .Database
            .MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
