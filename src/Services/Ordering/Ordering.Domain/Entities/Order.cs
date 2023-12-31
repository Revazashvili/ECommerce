using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Services.Common.Domain;

namespace Ordering.Domain.Entities;

public class Order : Entity
{
    private Order() { }
    private Order(Guid orderNumber, Guid userId, Address address, OrderStatus orderStatus, DateTime orderingDate)
    {
        OrderNumber = orderNumber;
        UserId = userId;
        Address = address;
        OrderStatus = orderStatus;
        OrderingDate = orderingDate;
        OrderItems = new List<OrderItem>();
    }
    
    public Guid OrderNumber { get; private set; }
    public Guid UserId { get; set; }
    public List<OrderItem> OrderItems { get; private set; }
    public Address Address { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public DateTime OrderingDate { get; private set; }
    
    public static Order Create(Guid userId,Address address)
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
    
    public void SetCreatedStatus()
    {
        OrderStatus = OrderStatus.Created;
    }
    
    public void SetUnAvailableQuantityStatus()
    {
        if (OrderStatus != OrderStatus.Pending)
            throw new OrderStatusException(OrderStatus, OrderStatus.UnAvailableQuantity);
        OrderStatus = OrderStatus.UnAvailableQuantity;

        AddDomainEvent(new SetOrderUnAvailableQuantityStatusDomainEvent(OrderNumber));
    }
    
    public void SetAvailableQuantityStatus()
    {
        if (OrderStatus != OrderStatus.Pending)
            throw new OrderStatusException(OrderStatus, OrderStatus.AvailableQuantity);
        OrderStatus = OrderStatus.AvailableQuantity;

        AddDomainEvent(new SetOrderAvailableQuantityStatusDomainEvent(OrderNumber));
    }
    
    public void SetPaidStatus()
    {
        if (OrderStatus != OrderStatus.AvailableQuantity)
            throw new OrderStatusException(OrderStatus, OrderStatus.Paid);
        OrderStatus = OrderStatus.Paid;

        AddDomainEvent(new SetOrderStatusPaidDomainEvent(this));
    }
    
    public void SetPendingStatus()
    {
        if (OrderStatus != OrderStatus.Created)
            throw new OrderStatusException(OrderStatus, OrderStatus.Pending);

        OrderStatus = OrderStatus.Pending;

        AddDomainEvent(new SetOrderPendingStatusDomainEvent(OrderNumber));
    }
}