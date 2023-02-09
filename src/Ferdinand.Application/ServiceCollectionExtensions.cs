using Ferdinand.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ferdinand.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddMediatR(typeof(AssemblyMarker))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            // .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))
            ;

        return services;
    }
}
