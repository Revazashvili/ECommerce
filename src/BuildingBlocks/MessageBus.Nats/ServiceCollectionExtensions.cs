using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus.Nats;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNatsMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        var natsSection = configuration.GetSection("Nats");

        var options = new NatsOptions(natsSection["ServerUrl"]!);
        services.AddScoped<IMessageBus>(_ => new NatsMessageBus(options));
        
        return services;
    }
}