using Ferdinand.Domain.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Ferdinand.Extensions.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureNServiceBus(this IHostBuilder builder, string endpointName)
    {
        builder.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var transportConnectionString = context.Configuration.GetConnectionString("RabbitMQ");
        
            endpointConfiguration.SendOnly();
            endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            endpointConfiguration
                .Conventions()
                .DefiningMessagesAs(IsMessage)
                .DefiningCommandsAs(IsCommand)
                .DefiningEventsAs(IsEvent);

            endpointConfiguration
                .UseTransport<RabbitMQTransport>()
                .ConnectionString(transportConnectionString)
                .UseConventionalRoutingTopology(QueueType.Quorum);

            return endpointConfiguration;
        });
        
        return builder;
    }
    
    private static bool IsMessage(this Type type)
    {
        return type.IsEvent() || type.IsCommand() || typeof(IMessage).IsAssignableFrom(type);
    }
    
    private static bool IsCommand(this Type type)
    {
        return typeof(ICommand).IsAssignableFrom(type);
    }

    private static bool IsEvent(this Type type)
    {
        return typeof(IDomainEvent).IsAssignableFrom(type) || typeof(IEvent).IsAssignableFrom(type);
    }
}
