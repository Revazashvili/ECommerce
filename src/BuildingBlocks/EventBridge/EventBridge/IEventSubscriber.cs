namespace EventBridge;

public interface IEventSubscriber;

public interface IIntegrationEventSubscriber : IEventSubscriber
{
    void Subscribe<TEvent, TEventHandler>(string topic)
        where TEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TEvent>;
}