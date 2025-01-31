using Microsoft.AspNetCore.Http;

namespace OrderPaymentSystem.Orders.Domain.Exceptions;

[ProblemDetails(
Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
Title = "Not Found",
StatusCode = StatusCodes.Status404NotFound
)]
public class EntityNotFoundException(
    string entityName,
    string fieldName,
    object fieldValue) : Exception(FormatMessage(entityName, fieldName, fieldValue)), IEntityException
{
    public string EntityName { get; } = entityName;
    public string FieldName { get; } = fieldName;
    public object FieldValue { get; } = fieldValue;

    private static string FormatMessage(
        string entityName,
        string fieldName,
        object fieldValue)
    {
        return $"Entity {entityName} with {fieldName} '{fieldValue}' not found.";
    }
}
