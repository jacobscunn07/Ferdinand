namespace Ferdinand.Infrastructure.Configuration;

public class ConnectionStringOptions
{
    public const string Position = "ConnectionStrings";
    
    public string Postgres { get; set; } = string.Empty;
    public string RabbitMQ { get; set; } = string.Empty;
}
