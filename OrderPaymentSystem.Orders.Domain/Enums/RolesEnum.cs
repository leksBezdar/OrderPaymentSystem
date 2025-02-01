using OrderPaymentSystem.Orders.Domain.Common;

namespace OrderPaymentSystem.Orders.Domain.Enums;

public sealed class RolesEnum : SmartEnum<RolesEnum>
{
    // Предопределённые значения ролей
    public static readonly RolesEnum Admin = new("admin");
    public static readonly RolesEnum Merchant = new("merchant");
    public static readonly RolesEnum User = new("user");

    // Статический конструктор инициализирует данные
    static RolesEnum()
    {
        Initialize([Admin, Merchant, User]);
    }

    // Закрытый конструктор
    private RolesEnum(string value) : base(value) { }
}
