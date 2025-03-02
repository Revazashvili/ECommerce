using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBridge.Subscriber;

public class IntegrationEventSubscriber : IIntegrationEventSubscriber
{
    private readonly SubscriberEventProcessFunctionStore _subscriberEventProcessFunctionStore;

    internal IntegrationEventSubscriber(SubscriberEventProcessFunctionStore subscriberEventProcessFunctionStore)
    {
        _subscriberEventProcessFunctionStore = subscriberEventProcessFunctionStore;
    }
    
    public virtual void Subscribe<TEvent, TEventHandler>(string topic) where TEvent : IntegrationEvent where TEventHandler : IIntegrationEventHandler<TEvent>
    {
        _subscriberEventProcessFunctionStore.AddProcessEventFunction(topic, ProcessFunc);
        return;

        Task ProcessFunc(string payload, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var message = JsonConvert.DeserializeObject<TEvent>(payload)!;

            var handler = serviceProvider.GetRequiredService<TEventHandler>();

            return handler.HandleAsync(message, cancellationToken);
        }
    }
}