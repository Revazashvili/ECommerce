using EventBridge.Dispatcher;
using Microsoft.Extensions.DependencyInjection;

namespace EventBridge.Outbox;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutboxDispatcher(this IServiceCollection services, 
        Func<IServiceProvider, IOutboxMessageRepository> outboxMessageRepositoryFactory)
    {
        services.AddScoped(outboxMessageRepositoryFactory);
        services.AddScoped<IIntegrationEventDispatcher, OutboxIntegrationEventDispatcher>();
        
        return services;
    }
}