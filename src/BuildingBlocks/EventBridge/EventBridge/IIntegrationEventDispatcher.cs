namespace EventBridge;

public interface IIntegrationEventDispatcher : IEventDispatcher
{
    Task DispatchAsync(string topic, IntegrationEvent @event, CancellationToken cancellationToken);
}