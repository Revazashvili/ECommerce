namespace Report.API.Models;

public class Order
{
    public Guid OrderNumber { get; set; }
    public int UserId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public Address Address { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderingDate { get; set; }
}