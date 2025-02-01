using OrderPaymentSystem.Orders.Domain.Exceptions;

namespace OrderPaymentSystem.Orders.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    private static readonly string[] AllowedCurrencies = { "USD", "RUB", "EUR" };
    public static Money Create(decimal amount, string currency)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be positive");

        if (!AllowedCurrencies.Contains(currency))
            throw new DomainException($"Unsupported currency: {currency}");

        return new Money(amount, currency);
    }

    // TODO: подключить внешнее апи для конвертации валют
    public Money ConvertTo(string targetCurrency, decimal rate)
    {
        return this with
        {
            Amount = Amount * rate,
            Currency = targetCurrency
        };
    }
}