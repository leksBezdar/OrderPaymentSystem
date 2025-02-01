using OrderPaymentSystem.Orders.Domain.Common;
using OrderPaymentSystem.Orders.Domain.ValueObjects;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.CartAggregate;

public class CartItem : Entity<long>
{
    public long ProductId { get; }
    public Money Price { get; }
    public int Quantity { get; private set; }

    internal CartItem(long productId, Money price, int quantity)
    {
        ProductId = productId;
        Price = price;
        SetQuantity(quantity);
    }

    public void SetQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.");
        Quantity = quantity;
    }
}