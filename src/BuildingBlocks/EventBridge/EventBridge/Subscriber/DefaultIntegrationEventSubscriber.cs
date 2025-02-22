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
        ProcessEvent processFunc = (payload, serviceProvider, cancellationToken) =>
        {
            var message = (TEvent)JsonConvert.DeserializeObject(payload, typeof(TEvent))!;

            var handler = serviceProvider.GetRequiredService<TEventHandler>();

            return handler.HandleAsync(message, cancellationToken);
        };
        
        _subscriberEventProcessFunctionStore.AddProcessEventFunction(topic, processFunc);
    }
}