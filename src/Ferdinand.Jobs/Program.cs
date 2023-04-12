using Ferdinand.Jobs;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;

var host = Host.CreateDefaultBuilder()
    .UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration))
    .ConfigureServices((cxt, services) =>
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.AddJobs();
        });
        
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
    }).Build();

await host.RunAsync();
