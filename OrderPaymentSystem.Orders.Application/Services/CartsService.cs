using Microsoft.EntityFrameworkCore;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Models.Carts;
using OrderPaymentSystem.Orders.Domain;
using OrderPaymentSystem.Orders.Domain.Entities;

namespace OrderPaymentSystem.Orders.Application.Services
{
    public class CartsService(OrdersDbContext context) : ICartsService
    {
        public async Task<CartDTO> Create(CartDTO cart)
        {
            var cartEntity = new CartEntity();
            var cartSaveResult = await context.Carts.AddAsync(cartEntity);
            await context.SaveChangesAsync();

            var cartItems = cart.CartItems
                .Select(item => new CartItemEntity
                {
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    CartId = cartSaveResult.Entity.Id
                });

            await context.CartItems.AddRangeAsync(cartItems);
            await context.SaveChangesAsync();

            var res = await context.Carts
                .Include(x => x.CartItems)
                .FirstAsync(x => x.Id == cartSaveResult.Entity.Id);

            return new CartDTO()
            {
                Id = res.Id,
                CartItems = res.CartItems!.ConvertAll(item => new CartItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                })
            };
        }
    }
}