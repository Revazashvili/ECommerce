using Ordering.Domain.Exceptions;
using Services.Common.Domain;

namespace Ordering.Domain.Entities;

public class Order : Entity
{
    private Order() { }
    private Order(Guid orderNumber, int userId, Address address, OrderStatus orderStatus, DateTime orderingDate)
    {
        OrderNumber = orderNumber;
        UserId = userId;
        Address = address;
        OrderStatus = orderStatus;
        OrderingDate = orderingDate;
    }
    
    public Guid OrderNumber { get; private set; }
    public int UserId { get; set; }
    public List<OrderItem> OrderItems { get; private set; }
    public Address Address { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public DateTime OrderingDate { get; private set; }
    
    public static Order Create(int userId,Address address)
    {
        var orderNumber = Guid.NewGuid();
        return new Order(orderNumber, userId,address, OrderStatus.Created, DateTime.Now);
    }

    public void AddOrderItem(Guid productId,string productName,decimal price,int quantity,string pictureUrl)
    {
        var alreadyAddedOrderItem = OrderItems.FirstOrDefault(item => item.ProductId == productId);

        if (alreadyAddedOrderItem is not null)
            alreadyAddedOrderItem.IncreaseQuantity(quantity);

        var newOrderItem = OrderItem.Create(productId, productName, price, quantity, pictureUrl);
        OrderItems.Add(newOrderItem);
    }

    public void Cancel()
    {
        if (OrderStatus == OrderStatus.Paid)
            throw new OrderStatusException(OrderStatus.Paid, OrderStatus.Cancelled);

        OrderStatus = OrderStatus.Cancelled;
    }
}