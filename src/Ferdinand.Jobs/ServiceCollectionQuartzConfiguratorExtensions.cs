using Quartz;

namespace Ferdinand.Jobs;

public static class ServiceCollectionQuartzConfiguratorExtensions
{
    public static IServiceCollectionQuartzConfigurator AddJobs(this IServiceCollectionQuartzConfigurator quartz)
    {
        var jobKey = new JobKey("PublishOutboxMessageJob");
        quartz.AddJob<PublishOutboxMessageJob>(opts => opts.WithIdentity(jobKey));
        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("PublishOutboxMessageJob-trigger")
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(5)
                .RepeatForever()));

        return quartz;
    }
}
