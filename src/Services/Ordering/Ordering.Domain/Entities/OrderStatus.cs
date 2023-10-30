namespace Ordering.Domain.Entities;

public enum OrderStatus
{
    Created, 
    Pending,
    AvailableQuantity,
    UnAvailableQuantity,
    Paid,
    Cancelled
}