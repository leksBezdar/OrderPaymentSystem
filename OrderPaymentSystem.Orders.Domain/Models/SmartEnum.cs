namespace OrderPaymentSystem.Orders.Domain.Models;

public abstract class SmartEnum<T>(string value) where T : SmartEnum<T>
{
    private static IReadOnlyList<T>? _allValues;
    private static Dictionary<string, T>? _byValue;

    public string Value { get; } = value;

    protected static void Initialize(IReadOnlyList<T> values)
    {
        _allValues = values ?? throw new ArgumentNullException(nameof(values));
        _byValue = values.ToDictionary(v => v.Value);
    }

    public static IReadOnlyCollection<T> GetAll()
    {
        EnsureInitialized();
        // Не может быть null, так как EnsureInitialized проверяет их состояние
        return _allValues!;
    }

    public static T FromValue(string value)
    {
        EnsureInitialized();

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));

        // Не может быть null, так как EnsureInitialized проверяет их состояние
        return _byValue!.TryGetValue(value, out var result)
            ? result
            : throw new KeyNotFoundException($"Invalid value: {value}");
    }

    private static void EnsureInitialized()
    {
        if (_byValue == null)
            throw new InvalidOperationException($"SmartEnum {typeof(T).Name} is not initialized.");
    }

    public override string ToString() => Value;
}