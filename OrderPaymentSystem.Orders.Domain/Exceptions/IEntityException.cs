namespace OrderPaymentSystem.Orders.Domain.Exceptions;

public interface IEntityException
{
    string EntityName { get; }
    string FieldName { get; }
    object FieldValue { get; }
}