using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Ferdinand.Api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            title = GetTitle(exception),
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception)
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
    
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ValidationException => "Validation Error",
            _ => "Server Error"
        };

    private static IEnumerable<ValidationFailure>? GetErrors(Exception exception) =>
        exception is ValidationException validationException ? validationException.Errors : null;
}
