namespace OrderPaymentSystem.Orders.Domain.Exceptions
{
    public class EntityNotFoundException(string entityName, string fieldName, object fieldValue) : Exception($"{entityName} entity with {fieldName} {fieldValue} was not found in database")
    {
    }
}
