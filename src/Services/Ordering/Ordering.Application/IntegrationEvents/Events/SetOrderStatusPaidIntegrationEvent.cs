using EventBus;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class SetOrderStatusPaidIntegrationEvent(Order order) : IntegrationEvent
{
    public Order Order { get; } = order;
}