using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace EventBridge.Subscriber;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBridge<THostedService>(this IServiceCollection services,
        Action<IIntegrationEventSubscriber> subscriberAction)
        where THostedService : IntegrationEventSubscriberService
    {
        var store = new SubscriberEventProcessFunctionStore();
        var integrationEventSubscriber = new IntegrationEventSubscriber(store);
        
        subscriberAction(integrationEventSubscriber);
        
        services.AddSingleton<IIntegrationEventSubscriber>(_ => integrationEventSubscriber);
        services.AddHostedService<THostedService>();
        
        var handlers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.BaseType == typeof(IIntegrationEventHandler<>))
            .ToList();
        
        foreach (var handler in handlers)
            services.AddTransient(handler);

        return services;
    }
}