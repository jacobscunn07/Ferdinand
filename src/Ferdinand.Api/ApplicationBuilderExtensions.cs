using Ferdinand.Api.Middlewares;

namespace Ferdinand.Api;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
