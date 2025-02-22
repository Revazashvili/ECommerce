using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace EventBridge.Subscriber;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBridge<THostedService>(this IServiceCollection services)
        where THostedService : IntegrationEventSubscriberService
    {
        services.AddSingleton<SubscriberEventProcessFunctionStore>();
        services.AddSingleton<IIntegrationEventSubscriber, IntegrationEventSubscriber>();
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