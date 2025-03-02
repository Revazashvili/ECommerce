using Microsoft.Extensions.DependencyInjection;

namespace EventBridge.Subscriber;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBridge<THostedService>(this IServiceCollection services, 
        SubscriberConfiguration subscriberConfiguration)
        where THostedService : IntegrationEventSubscriberService
    {
        var store = new SubscriberEventProcessFunctionStore();
        var integrationEventSubscriber = new IntegrationEventSubscriber(store);
        
        subscriberConfiguration.Subscriber(integrationEventSubscriber);

        services.AddSingleton(store);
        services.AddSingleton<IIntegrationEventSubscriber>(_ => integrationEventSubscriber);
        services.AddHostedService<THostedService>();

        var handlers = subscriberConfiguration.AssembliesToRegister
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t =>
                t is { IsInterface: false, IsAbstract: false }
                && t
                    .GetInterfaces()
                    .Any(i => i.IsGenericType
                              && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
            )
            .ToList();
        
        foreach (var handler in handlers)
            services.AddTransient(handler);

        return services;
    }
}