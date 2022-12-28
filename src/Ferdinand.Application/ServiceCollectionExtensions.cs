using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ferdinand.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddMediatR(typeof(AssemblyMarker));

        return services;
    }
}
