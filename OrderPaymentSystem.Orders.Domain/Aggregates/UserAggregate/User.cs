using Microsoft.AspNetCore.Identity;
using OrderPaymentSystem.Orders.Domain.Exceptions;
using OrderPaymentSystem.Orders.Domain.ValueObjects;
using OrderPaymentSystem.Orders.Domain.Aggregates.OrderAggregate;
using OrderPaymentSystem.Orders.Domain.Aggregates.MerchantAgregate;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.UserAggregate;

public class User : IdentityUser<long>
{
    private readonly List<Order> _orders = [];

    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public Address? ShippingAddress { get; private set; }
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    // Для мерчантов
    public Merchant? OwnedMerchant { get; private set; }

    public void UpdateShippingAddress(Address address)
    {
        ShippingAddress = address;
    }

    public void AssignMerchant(Merchant merchant)
    {
        if (OwnedMerchant != null)
            throw new DomainException("User already owns a merchant");

        OwnedMerchant = merchant;
    }
}