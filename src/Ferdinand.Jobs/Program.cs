using Ferdinand.Application;
using Ferdinand.Data;
using Ferdinand.Extensions.Hosting;
using Ferdinand.Jobs;
using Microsoft.Extensions.Hosting;
using Serilog;
using AssemblyMarker = Ferdinand.Jobs.AssemblyMarker;

var host = Host.CreateDefaultBuilder()
    .UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration))
    .ConfigureNServiceBus(typeof(AssemblyMarker).Assembly.GetName().Name)
    .ConfigureServices((ctx, services) =>
    {
        services
            .AddDataServices(ctx.Configuration)
            .AddApplicationServices()
            .AddQuartz()
            ;
    })
    .Build();

await host.RunAsync();
