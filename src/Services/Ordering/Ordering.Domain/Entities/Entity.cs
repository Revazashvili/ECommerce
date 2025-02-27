using System.Text.Json.Serialization;
using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Entities;

public abstract class Entity
{
    private readonly List<INotification> _domainEvents = [];

    [JsonIgnore]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
    protected void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);
    public void ClearDomainEvents() => _domainEvents.Clear();
}