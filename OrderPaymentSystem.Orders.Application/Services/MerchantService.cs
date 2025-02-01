using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Models.Merchants;
using OrderPaymentSystem.Orders.Domain;
using OrderPaymentSystem.Orders.Domain.Entities;

namespace OrderPaymentSystem.Orders.Application.Services;

public class MerchantService(OrdersDbContext context) : IMerchantService
{
    public async Task<MerchantDTO> Create(MerchantDTO merchant)
    {
        var entity = new Merchant
        {
            Name = merchant.Name,
            Phone = merchant.Phone,
            WebSite = merchant.WebSite
        };

        var res = (await context.Merchants.AddAsync(entity)).Entity;
        await context.SaveChangesAsync();

        return new MerchantDTO
        {
            Id = res.Id,
            Name = res.Name,
            Phone = res.Phone,
            WebSite = res.WebSite
        };
    }
}
