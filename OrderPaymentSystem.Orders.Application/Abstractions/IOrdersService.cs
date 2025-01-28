using OrderPaymentSystem.Orders.Application.Models.Orders;

namespace OrderPaymentSystem.Orders.Application.Abstractions
{
    public interface IOrdersService
    {
        Task<OrderDTO> Create(CreateOrderDTO order);
        Task<OrderDTO> GetById(long orderId);
        Task<List<OrderDTO>> GetByUser(long cutomerId);
        Task<List<OrderDTO>> GetAll();
        Task Reject(long orderId);
    }
}
