using OrderPaymentSystem.Orders.Domain.Common;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.OrderAggregate;

public sealed class OrderStatusesEnum : SmartEnum<OrderStatusesEnum>
{
    // Предопределенные значения статусов заказа
    public static readonly OrderStatusesEnum New = new("new");
    public static readonly OrderStatusesEnum PendingPayment = new("pendingPayment");
    public static readonly OrderStatusesEnum Paid = new("paid");
    public static readonly OrderStatusesEnum InProcess = new("inProcess");
    public static readonly OrderStatusesEnum Done = new("done");
    public static readonly OrderStatusesEnum Failed = new("failed");
    public static readonly OrderStatusesEnum Cancelled = new("cancelled");
    public static readonly OrderStatusesEnum Refunded = new("refunded");

    static OrderStatusesEnum()
    {
        Initialize([New, PendingPayment, Paid, InProcess, Done, Failed, Cancelled, Refunded]);
    }

    private OrderStatusesEnum(string value) : base(value) { }
}