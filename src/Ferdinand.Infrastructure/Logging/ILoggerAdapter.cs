namespace Ferdinand.Infrastructure.Logging;

public interface ILoggerAdapter<T>
{
    public void LogInformation(string? message, params object?[] args);
    
    public void LogDebug(string? message, params object?[] args);

    void LogError(Exception? exception, string? message, params object?[] args);
}
