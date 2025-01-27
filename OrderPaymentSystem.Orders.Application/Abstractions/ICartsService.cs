using OrderPaymentSystem.Orders.Application.Models.Carts;

namespace OrderPaymentSystem.Orders.Application.Abstractions
{
    public interface ICartsService
    {
        Task<CartDTO> Create(CartDTO cart);
    }
}
