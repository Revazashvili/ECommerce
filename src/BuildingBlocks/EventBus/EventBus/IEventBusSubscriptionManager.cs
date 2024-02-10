namespace EventBus;

/// <summary>
/// Interface for managing subscription for event bus.
/// </summary>
public interface IEventBusSubscriptionManager
{
    /// <summary>
    /// Add subscription handler of type <see cref="TH"/> for event type of <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of event.</typeparam>
    /// <typeparam name="TH">Type of event handler.</typeparam>
    void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    
    /// <summary>
    /// Gets handlers for type of event.
    /// </summary>
    /// <typeparam name="T">type of event for which handlers will be retrieved.</typeparam>
    /// <returns>IEnumerable of handlers.</returns>
    IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent;
}