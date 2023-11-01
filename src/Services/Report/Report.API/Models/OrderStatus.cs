namespace Report.API.Models;

public enum OrderStatus
{
    Created, 
    Pending,
    AvailableQuantity,
    UnAvailableQuantity,
    Paid,
    Cancelled
}