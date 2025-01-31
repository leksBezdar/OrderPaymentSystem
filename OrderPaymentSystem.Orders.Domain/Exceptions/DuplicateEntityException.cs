namespace OrderPaymentSystem.Orders.Domain.Exceptions;

public class DuplicateEntityException(
    string entityName,
    string fieldName,
    object fieldValue,
    string? additionalInfo = null) : Exception(FormatMessage(entityName, fieldName, fieldValue, additionalInfo))
{
    public string EntityName { get; } = entityName;
    public string FieldName { get; } = fieldName;
    public object FieldValue { get; } = fieldValue;

    private static string FormatMessage(string entityName, string fieldName, object fieldValue, string? additionalInfo)
    {
        var message = $"Entity '{entityName}' with {fieldName} = '{fieldValue}' already exists.";
        return additionalInfo is not null ? $"{message} {additionalInfo}" : message;
    }
}
