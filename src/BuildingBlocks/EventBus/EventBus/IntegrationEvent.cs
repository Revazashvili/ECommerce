using Newtonsoft.Json;

namespace EventBus;

/// <summary>
/// Marker class for events.
/// </summary>
public class IntegrationEvent(Guid id,DateTime creationDate)
{
    public IntegrationEvent() : this(Guid.NewGuid(), DateTime.UtcNow) { }

    /// <summary>
    /// Identifier for event.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; } = id;

    /// <summary>
    /// Event creation date.
    /// </summary>
    [JsonProperty("creation_date")]
    public DateTime CreationDate { get; init; } = creationDate;
}