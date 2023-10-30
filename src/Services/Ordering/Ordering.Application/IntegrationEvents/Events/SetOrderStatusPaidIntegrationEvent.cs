using EventBus;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.Events;

public class SetOrderStatusPaidIntegrationEvent : IntegrationEvent
{
    public SetOrderStatusPaidIntegrationEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
}