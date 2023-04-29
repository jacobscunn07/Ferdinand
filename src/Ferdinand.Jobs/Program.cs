using Ferdinand.Application;
using Ferdinand.Infrastructure;
using Ferdinand.Extensions.Hosting;
using Ferdinand.Infrastructure.Logging;
using Ferdinand.Infrastructure.Messaging;
using Ferdinand.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using AssemblyMarker = Ferdinand.Jobs.AssemblyMarker;

var host = Host.CreateDefaultBuilder()
    .UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration))
    .ConfigureNServiceBus(typeof(AssemblyMarker).Assembly.GetName().Name!)
    .ConfigureServices((ctx, services) =>
    {
        services
            .AddDataServices(ctx.Configuration)
            .AddApplicationServices()
            .AddQuartz()
            .AddSingleton<IMessageBus, NServiceBusMessageBus>()
            .AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
            ;
    })
    .Build();

await host.RunAsync();
