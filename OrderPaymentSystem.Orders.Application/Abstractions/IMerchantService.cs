using OrderPaymentSystem.Orders.Application.Models.Merchants;

namespace OrderPaymentSystem.Orders.Application.Abstractions;

public interface IMerchantService
{
    Task<MerchantDTO> Create(MerchantDTO merchant);
}
