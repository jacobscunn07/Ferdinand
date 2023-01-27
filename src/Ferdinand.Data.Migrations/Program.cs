using Ferdinand.Application;
using Ferdinand.Data;
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

await host.RunAsync();
