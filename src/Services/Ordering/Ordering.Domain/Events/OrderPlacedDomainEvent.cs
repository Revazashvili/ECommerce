using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public record OrderPlacedDomainEvent(Guid OrderNumber, Guid UserId) : INotification;