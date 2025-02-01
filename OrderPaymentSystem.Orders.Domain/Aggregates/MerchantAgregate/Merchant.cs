using OrderPaymentSystem.Orders.Domain.Common;
using OrderPaymentSystem.Orders.Domain.ValueObjects;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.MerchantAgregate;

public class Merchant(string name, string description) : AuditableEntity<long>
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    private readonly List<Product> _products = [];
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    public Product AddProduct(string name, Money price, int stock)
    {
        var product = new Product(name, price, Id, stock);
        _products.Add(product);
        return product;
    }

    public void RemoveProduct(long productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product != null) _products.Remove(product);
    }
}