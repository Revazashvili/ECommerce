namespace EventBridge;

/// <summary>
/// IntegrationEvent event class for marking events
/// </summary>
/// <param name="id">unique id of event</param>
/// <param name="creationDate">creation date of event</param>
public class IntegrationEvent<T> where T : notnull
{
    protected IntegrationEvent(T aggregateId, string topic)
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        Topic = topic;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Identifier for event.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Aggregate Identifier of event
    /// </summary>
    public T AggregateId { get; init; }
    
    /// <summary>
    /// topic of event
    /// </summary>
    public string Topic { get; init; }

    /// <summary>
    /// Event creation date.
    /// </summary>
    public DateTime Timestamp { get; init; }
}