using OrderPaymentSystem.Orders.Domain.Common;
using OrderPaymentSystem.Orders.Domain.ValueObjects;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.CartAggregate;

public class Cart : AuditableEntity<long>
{
    public long UserId { get; }
    private readonly List<CartItem> _items = [];
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public Money Total => CalculateTotal();

    private Cart() { }

    public Cart(long userId) => UserId = userId;

    public void AddItem(long productId, Money price, int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.SetQuantity(quantity);
            return;
        }

        _items.Add(new CartItem(productId, price, quantity));
    }

    public void RemoveItem(long productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null) _items.Remove(item);
    }

    private Money CalculateTotal()
    {
        // TODO: убрать моковые данные, реализовать CalculateTotal
        return _items.Aggregate(
            new Money(0, "USD"),
            (total, item) => total with { Amount = total.Amount + item.Price.Amount * item.Quantity });
    }

    // TODO: возможно, нужно выбрасывать ошибку при очистке корзины, если какие-то заказы находятся в каком-то статусе, вроде PendingPayment
    public void Clear()
    {
        _items.Clear();
    }
}