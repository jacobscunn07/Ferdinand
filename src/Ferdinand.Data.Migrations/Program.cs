using Ferdinand.Infrastructure.EntityFrameworkCore;
using Ferdinand.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) => 
       {
            services
                .AddHostedService<MigrationRunnerService>()
                .AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
                .AddDbContext<FerdinandDbContext>((sp, opts) =>
                {
                    var _connectionString = ctx.Configuration.GetConnectionString("Postgres");
                    opts.UseNpgsql(_connectionString);
                }, ServiceLifetime.Singleton)
                ;
       }
    ).Build();

await host.RunAsync();
