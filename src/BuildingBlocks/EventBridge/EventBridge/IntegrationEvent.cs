namespace EventBridge;

/// <summary>
/// IntegrationEvent event class for marking events
/// </summary>
/// <param name="id">unique id of event</param>
/// <param name="creationDate">creation date of event</param>
public class IntegrationEvent
{
    public IntegrationEvent() { }

    public IntegrationEvent(string aggregateId)
    {
        AggregateId = aggregateId;
        Timestamp = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Identifier for event.
    /// </summary>
    public long Id { get; init; }
    
    /// <summary>
    /// Aggregate Identifier of event
    /// </summary>
    public string AggregateId { get; init; }

    /// <summary>
    /// Event creation date.
    /// </summary>
    public DateTime Timestamp { get; init; }
}