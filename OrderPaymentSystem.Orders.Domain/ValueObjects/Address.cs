using OrderPaymentSystem.Orders.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace OrderPaymentSystem.Orders.Domain.ValueObjects;

public record Address
{
    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string ZipCode { get; }
    public string PhoneNumber { get; }

    private Address(string country, string city, string street, string zipCode, string phoneNumber)
    {
        // Валидация
        // TODO: вынести валидацию в отдельный метод и поискать способ вызова этого метода автоматически
        // Что-то вроде Validate() в PostInit()
        if (string.IsNullOrWhiteSpace(country)) throw new DomainException("Country is required");
        if (!IsValidPhoneNumber(phoneNumber)) throw new DomainException("Invalid phone number");

        Country = country;
        City = city;
        Street = street;
        ZipCode = zipCode;
        PhoneNumber = phoneNumber;
    }
    public static Address Create(string country, string city,
        string street, string zipCode, string phoneNumber)
    {
        return new Address(country, city, street, zipCode, phoneNumber);
    }
    private static bool IsValidPhoneNumber(string phoneNumber)
    => Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$");
}