namespace OrderPaymentSystem.Orders.Domain.Models
{
    /// <summary>
    /// Smart Enum паттерн для ролей пользователей.
    /// </summary>
    public sealed class RolesEnum
    {
        // Предопределённые значения ролей
        public static readonly RolesEnum Admin = new("admin");
        public static readonly RolesEnum Merchant = new("merchant");
        public static readonly RolesEnum User = new("user");

        private static readonly IReadOnlyList<RolesEnum> _allRoles = [Admin, Merchant, User];
        private static readonly Dictionary<string, RolesEnum> _rolesByValue = _allRoles.ToDictionary(role => role.Value);

        /// <summary>
        /// Значение роли.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Закрытый конструктор для ограничения создания экземпляров.
        /// </summary>
        private RolesEnum(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Возвращает список всех ролей.
        /// </summary>
        public static IReadOnlyCollection<RolesEnum> GetAll() => _allRoles;

        /// <summary>
        /// Получает роль по значению.
        /// </summary>
        public static RolesEnum FromValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Role value cannot be null or whitespace.", nameof(value));

            return _rolesByValue.TryGetValue(value, out var role)
                ? role
                : throw new KeyNotFoundException($"Invalid role value: {value}");
        }

        public override string ToString() => Value;
    }
}
