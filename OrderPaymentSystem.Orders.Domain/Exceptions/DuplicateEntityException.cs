using Microsoft.AspNetCore.Http;

namespace OrderPaymentSystem.Orders.Domain.Exceptions;

[ProblemDetails(
    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
    Title = "Conflict",
    StatusCode = StatusCodes.Status409Conflict
)]
public class DuplicateEntityException(
    string entityName,
    string fieldName,
    object fieldValue,
    string? additionalInfo = null) : Exception(FormatMessage(entityName, fieldName, fieldValue, additionalInfo)), IEntityException
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
