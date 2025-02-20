namespace EventBridge;

public interface IIntegrationEventDispatcher : IEventDispatcher
{
    Task DispatchAsync<TKey>(IntegrationEvent<TKey> @event, CancellationToken cancellationToken);
}