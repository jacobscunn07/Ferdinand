using Ferdinand.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ferdinand.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<FerdinandDbContext>(opts => 
                opts.UseNpgsql(configuration.GetConnectionString("Postgres")));
        
        return services;
    }
}
