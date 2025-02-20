using Contracts.Mediatr.Wrappers;

namespace Ordering.Domain.Events;

public record OrderPlaceDomainEvent(Guid OrderNumber, Guid UserId) : INotification;