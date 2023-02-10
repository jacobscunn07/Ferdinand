using Ferdinand.Data.EntityFrameworkCore;
using Ferdinand.Data.EntityFrameworkCore.Interceptors;
using Ferdinand.Data.EntityFrameworkCore.Repositories;
using Ferdinand.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ferdinand.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>()
            .AddDbContext<FerdinandDbContext>((sp, opts) =>
            {
                var convertDomainEventsToOutboxMessagesInterceptor =
                    sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

                var _connectionString = configuration.GetConnectionString("Postgres");
                opts.UseNpgsql(_connectionString)
                    .AddInterceptors(convertDomainEventsToOutboxMessagesInterceptor);
            })
            .AddScoped<IColorRepository, ColorRepository>();
        
        return services;
    }
}
