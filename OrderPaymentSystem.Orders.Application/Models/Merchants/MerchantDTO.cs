namespace OrderPaymentSystem.Orders.Application.Models.Merchants;

public class MerchantDTO
{
    public long? Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? WebSite { get; set; }
}
