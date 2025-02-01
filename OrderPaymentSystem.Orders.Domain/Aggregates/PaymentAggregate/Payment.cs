using OrderPaymentSystem.Orders.Domain.Aggregates.OrderAggregate;
using OrderPaymentSystem.Orders.Domain.Common;
using OrderPaymentSystem.Orders.Domain.Exceptions;
using OrderPaymentSystem.Orders.Domain.ValueObjects;

namespace OrderPaymentSystem.Orders.Domain.Aggregates.PaymentAggregate;

public class Payment : AuditableEntity<long>
{
    public long OrderId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatusEnum Status { get; private set; }

    private Payment() { }

    public Payment(Order order)
    {
        OrderId = order.Id;
        Amount = order.TotalAmount;
        Status = PaymentStatusEnum.Pending;
    }

    public void MarkAsPaid() => Status = PaymentStatusEnum.Success;
    public void MarkAsFailed() => Status = PaymentStatusEnum.Failed;
    public void Refund() => Status = PaymentStatusEnum.Refunded;

    public static Payment Create(Order order)
    {
        if (order.Status != OrderStatusesEnum.PendingPayment)
            throw new DomainException("Order not ready for payment");

        return new Payment
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            Status = PaymentStatusEnum.Pending
        };
    }

    public void ChangeStatus(PaymentStatusEnum newStatus)
    {
        if (!PaymentStatusEnum.IsTransitionAllowed(Status, newStatus))
            throw new DomainException($"Invalid status transition from {Status} to {newStatus}");

        Status = newStatus;
    }
}
