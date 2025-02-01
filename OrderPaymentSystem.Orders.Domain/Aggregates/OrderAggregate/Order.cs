using OrderPaymentSystem.Orders.Domain.Aggregates.CartAggregate;
using OrderPaymentSystem.Orders.Domain.Aggregates.PaymentAggregate;
using OrderPaymentSystem.Orders.Domain.Common;
using OrderPaymentSystem.Orders.Domain.Exceptions;
using OrderPaymentSystem.Orders.Domain.ValueObjects;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.OrderAggregate;

public sealed class Order : AuditableEntity<long>
{
    public long UserId { get; private set; }
    public long CartId { get; private set; }
    public OrderStatusesEnum Status { get; private set; } = OrderStatusesEnum.New;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money TotalAmount { get; private set; }
    public Address ShippingAddress { get; private set; }
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();
    private readonly List<OrderItem> _items = [];
    private readonly List<Payment> _payments = [];

    private Order()
    {
        // Инициализация по умолчанию
        TotalAmount = new Money(0, "USD");
        ShippingAddress = Address.Create("Unknown", "Unknown", "Unknown", "00000", "0000000000");
    }

    public static Order Create(long userId, Cart cart, Address shippingAddress)
    {
        if (cart.Items.Count == 0)
            throw new DomainException("Cart is empty.");

        var order = new Order
        {
            UserId = userId,
            CartId = cart.Id,
            ShippingAddress = shippingAddress
        };

        foreach (var item in cart.Items)
        {
            order._items.Add(new OrderItem(
                item.ProductId,
                item.Price,
                item.Quantity
            ));
        }

        order.CalculateTotal(); // Рассчитываем итоговую сумму
        cart.Clear(); // Очищаем корзину после создания заказа

        return order;
    }

    public void Cancel()
    {
        if (Status != OrderStatusesEnum.New && Status != OrderStatusesEnum.PendingPayment)
            throw new DomainException("Cannot cancel order in current status");

        Status = OrderStatusesEnum.Cancelled;
    }

    public void CalculateTotal()
    {
        TotalAmount = Items.Aggregate(
            new Money(0, "USD"),
            (total, item) => total with { Amount = total.Amount + item.Price.Amount * item.Quantity });
    }

    public void Ship()
    {
        if (Status != OrderStatusesEnum.Paid)
            throw new DomainException("Cannot ship unpaid order");

        Status = OrderStatusesEnum.Done;
    }

    public void ProcessPayment(Payment payment)
    {
        if (payment.Status != PaymentStatusEnum.Success)
            throw new DomainException("Payment not completed");

        if (Status != OrderStatusesEnum.PendingPayment)
            throw new DomainException("Order not ready for payment");

        ValidateCurrencyConsistency();

        Status = OrderStatusesEnum.Paid;
        payment.MarkAsPaid();
    }

    // TODO: Сделать какое-то приведение валют
    private void ValidateCurrencyConsistency()
    {
        if (Items.Any(i => i.Price.Currency != TotalAmount.Currency))
            throw new DomainException("Mixed currencies in order items");
    }
    public void AddPayment(Payment payment)
    {
        if (payment.Amount != TotalAmount)
            throw new DomainException("Payment amount mismatch");

        _payments.Add(payment);
    }
}