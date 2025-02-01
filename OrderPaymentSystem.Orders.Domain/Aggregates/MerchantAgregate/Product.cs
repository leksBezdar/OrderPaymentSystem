using OrderPaymentSystem.Orders.Domain.Common;
using OrderPaymentSystem.Orders.Domain.Exceptions;
using OrderPaymentSystem.Orders.Domain.ValueObjects;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.MerchantAgregate;

public class Product : AuditableEntity<long>
{
    public string Name { get; }
    public Money Price { get; }
    public long MerchantId { get; }
    public int StockQuantity { get; private set; }

    public Product(string name, Money price, long merchantId, int stockQuantity)
    {
        if (price.Amount <= 0)
            throw new DomainException("Price must be positive.");
        if (stockQuantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");

        Name = name;
        Price = price;
        MerchantId = merchantId;
        StockQuantity = stockQuantity;
    }

    public void UpdateStock(int quantity)
    {
        if (StockQuantity + quantity < 0)
            throw new DomainException("Insufficient stock.");
        StockQuantity += quantity;
    }
}