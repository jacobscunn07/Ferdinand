using Ferdinand.Api.Middlewares;

namespace Ferdinand.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        return services.AddTransient<ExceptionHandlingMiddleware>();
    }
}
