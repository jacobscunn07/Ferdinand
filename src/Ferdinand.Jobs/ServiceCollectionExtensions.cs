using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Ferdinand.Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services
            .AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.AddJobs();
            })
            .AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

        return services;
    }
}