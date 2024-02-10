namespace EventBus;

/// <summary>
/// Marker interface for handling events.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIntegrationEventHandler<in T>
    where T : IntegrationEvent
{
    /// <summary>
    /// Method used to handle subscribed event.
    /// </summary>
    /// <param name="event">Type of event to be handled.</param>
    /// <returns>Response type of <see cref="Task"/>.</returns>
    Task Handle(T @event);
}