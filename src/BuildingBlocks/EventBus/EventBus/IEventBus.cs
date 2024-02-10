namespace EventBus;

/// <summary>
/// Interface for interacting with events
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Asynchronously send a single message to a event bus.
    /// </summary>
    /// <param name="event">Event to be sent in event bus.</param>
    /// <typeparam name="T">Event type of <see cref="IntegrationEvent"/>.</typeparam>
    /// <returns>Response type of <see cref="Task"/>.</returns>
    Task PublishAsync<T>(T @event) where T : IntegrationEvent;
    
    /// <summary>
    /// Subscribes to a single topic name of <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Event which will be subscribed. name of <see cref="T"/> will be used as topic name.</typeparam>
    /// <typeparam name="TH">Handler for <see cref="T"/> event.</typeparam>
    /// <returns>Response type of <see cref="Task"/>.</returns>
    Task SubscribeAsync<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}