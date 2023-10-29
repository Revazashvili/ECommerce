using System.Text.Json.Serialization;
using Contracts.Mediatr.Wrappers;

namespace Products.Domain.Models;

public abstract class Entity
{
    private List<INotification> _domainEvents = new();

    [JsonIgnore]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
    public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);
    public void ClearDomainEvents() => _domainEvents.Clear();
}