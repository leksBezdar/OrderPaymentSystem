namespace OrderPaymentSystem.Orders.Domain.Common;

public abstract class AuditableEntity<TId> : Entity<TId>
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public bool IsActive { get; protected set; } = true;
}
