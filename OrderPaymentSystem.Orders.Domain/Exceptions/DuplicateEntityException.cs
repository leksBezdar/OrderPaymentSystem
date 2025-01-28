namespace OrderPaymentSystem.Orders.Domain.Exceptions;

public class DuplicateEntityException(string fieldName, object fieldValue) : Exception($"{fieldName} with value {fieldValue} already exists")
{

}