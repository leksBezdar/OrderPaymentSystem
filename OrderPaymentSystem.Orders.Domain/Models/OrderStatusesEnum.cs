namespace OrderPaymentSystem.Orders.Domain.Models;

public sealed class OrderStatusesEnum : SmartEnum<OrderStatusesEnum>
{
    // Предопределенные значения статусов заказа
    public static readonly OrderStatusesEnum New = new("new");
    public static readonly OrderStatusesEnum PendingPayment = new("pendingPayment");
    public static readonly OrderStatusesEnum InProcess = new("inProcess");
    public static readonly OrderStatusesEnum Done = new("done");
    public static readonly OrderStatusesEnum Failed = new("failed");
    public static readonly OrderStatusesEnum Cancelled = new("cancelled");
    public static readonly OrderStatusesEnum Refunded = new("refunded");

    // Статический конструктор инициализирует данные
    static OrderStatusesEnum()
    {
        Initialize([New, PendingPayment, InProcess, Done, Failed, Cancelled, Refunded]);
    }

    // Закрытый конструктор
    private OrderStatusesEnum(string value) : base(value) { }
}