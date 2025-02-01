using Microsoft.AspNetCore.Http;

namespace OrderPaymentSystem.Orders.Domain.Exceptions;

/// <summary>
/// Базовое исключение для всех доменных ошибок.
/// Наследники должны добавлять атрибут <see cref="ProblemDetailsAttribute"/>.
/// </summary>
[ProblemDetails(
    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
    Title = "Domain Validation Error",
    StatusCode = StatusCodes.Status400BadRequest,
    IncludeEntityInfo = false
)]
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DomainException() : base()
    {
    }
}