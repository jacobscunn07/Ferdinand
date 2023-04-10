using Ferdinand.Application;
using Ferdinand.Data;
using Ferdinand.Data.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) => 
       {
            services
                .AddHostedService<MigrationRunnerService>()
                .AddDataServices(ctx.Configuration)
                .AddApplicationServices();
       }
    ).Build();

var ctx = host.Services.GetRequiredService<FerdinandDbContext>();
await ctx.Database.EnsureCreatedAsync();

await host.RunAsync();
