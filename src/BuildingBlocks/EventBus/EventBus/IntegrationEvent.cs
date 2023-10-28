using Newtonsoft.Json;

namespace EventBus;

public class IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;           
    }
    
    public IntegrationEvent(Guid id,DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }

    [JsonProperty("id")]
    public Guid Id { get; init; }
    [JsonProperty("creation_date")]
    public DateTime CreationDate { get; init; }
}