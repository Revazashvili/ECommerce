using Ordering.Domain.Exceptions;

namespace Ordering.Domain.Entities;

public class Order : Entity
{
    public Guid OrderNumber { get; init; }
    public Guid UserId { get; init; }
    public List<OrderItem> OrderItems { get; init; }
    public Address Address { get; init; }
    public OrderStatus OrderStatus { get; set; }
    public DateTimeOffset OrderingDate { get; init; }

    private Order() {}
    
    private Order(Guid userId,Address address)
    {
        OrderNumber = Guid.NewGuid();
        UserId = userId;
        Address = address;
        OrderStatus = OrderStatus.Created;
        OrderingDate = DateTime.UtcNow;
        OrderItems = [];
    }
    
    public static Order Create(Guid userId,Address address)
    {
        return new Order(userId, address);
    }

    public void AddOrderItem(Guid productId,string productName,decimal price,int quantity,string pictureUrl)
    {
        var alreadyAddedOrderItem = OrderItems.FirstOrDefault(item => item.ProductId == productId);

        alreadyAddedOrderItem?.IncreaseQuantity(quantity);

        var newOrderItem = OrderItem.Create(productId, productName, price, quantity, pictureUrl);
        OrderItems.Add(newOrderItem);
    }

    public void Cancel()
    {
        if (OrderStatus == OrderStatus.Paid)
            throw new OrderStatusException(OrderStatus.Paid, OrderStatus.Cancelled);

        OrderStatus = OrderStatus.Cancelled;
    }
    
    public void SetCreatedStatus()
    {
        OrderStatus = OrderStatus.Created;
    }
    
    public void SetPaidStatus()
    {
        if (OrderStatus != OrderStatus.AvailableQuantity)
            throw new OrderStatusException(OrderStatus, OrderStatus.Paid);
        
        OrderStatus = OrderStatus.Paid;
    }
}