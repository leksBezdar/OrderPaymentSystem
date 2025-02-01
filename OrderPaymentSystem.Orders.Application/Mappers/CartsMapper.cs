using OrderPaymentSystem.Orders.Application.Models.Carts;
using OrderPaymentSystem.Orders.Domain.Aggregates.CartAggregate;
using OrderPaymentSystem.Orders.Domain.Entities;

namespace OrderPaymentSystem.Orders.Application.Mappers;

public static class CartsMapper
{
    public static CartDTO ToDTO(this Cart cart)
    {
        return new CartDTO()
        {
            Id = cart.Id,
            CartItems = cart.CartItems!.ConvertAll(item => new CartItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
            })
        };
    }

    public static Cart ToEntity(this CartDTO cart)
    {
        return new Cart()
        {
            CartItems = cart.CartItems.ConvertAll(cart => cart.ToEntity())
        };
    }

    public static CartItemEntity ToEntity(this CartItemDTO item)
    {
        return new CartItemEntity
        {
            Name = item.Name,
            Price = item.Price,
            Quantity = item.Quantity,
        };
    }
}
