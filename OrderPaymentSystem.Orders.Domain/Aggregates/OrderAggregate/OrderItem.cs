using OrderPaymentSystem.Orders.Domain.Common;
using OrderPaymentSystem.Orders.Domain.ValueObjects;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.OrderAggregate;

public class OrderItem : Entity<long>
{
    public long ProductId { get; }
    public Money Price { get; }
    public int Quantity { get; }

    internal OrderItem(long productId, Money price, int quantity)
    {
        ProductId = productId;
        Price = price;
        Quantity = quantity;
    }
}