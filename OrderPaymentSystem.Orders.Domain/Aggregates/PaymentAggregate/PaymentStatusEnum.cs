using OrderPaymentSystem.Orders.Domain.Common;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.PaymentAggregate;

public sealed class PaymentStatusEnum : SmartEnum<PaymentStatusEnum>
{
    public static readonly PaymentStatusEnum Pending = new("Pending");
    public static readonly PaymentStatusEnum Success = new("Success");
    public static readonly PaymentStatusEnum Failed = new("Failed");
    public static readonly PaymentStatusEnum Refunded = new("Refunded");

    static PaymentStatusEnum()
    {
        Initialize([Pending, Success, Failed, Refunded]);
    }

    private PaymentStatusEnum(string value) : base(value) { }

    public static bool IsTransitionAllowed(PaymentStatusEnum current, PaymentStatusEnum newStatus)
    {
        return AllowedTransitions[current].Contains(newStatus);
    }


    // TODO: Возможно, плохая идея хранить так жестко заданные переходы. 
    private static readonly Dictionary<PaymentStatusEnum, List<PaymentStatusEnum>> AllowedTransitions = new()
    {
        [Pending] = [Success, Failed],
        [Success] = [Refunded],
        [Failed] = [Pending],
        [Refunded] = []
    };
}