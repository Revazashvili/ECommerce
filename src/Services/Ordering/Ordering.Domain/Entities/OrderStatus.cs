namespace Ordering.Domain.Entities;

public enum OrderStatus
{
    Created, 
    Pending,
    StockAvailable,
    StockUnAvailable,
    Paid,
    Cancelled
}