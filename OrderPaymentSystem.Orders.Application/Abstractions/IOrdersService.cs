using OrderPaymentSystem.Orders.Application.Models.Orders;

namespace OrderPaymentSystem.Orders.Application.Abstractions
{
    public interface IOrdersService
    {
        Task<OrderDTO> Create(CreateOrderDTO order);
        Task<OrderDTO> GetById(Guid orderId);
        Task<List<OrderDTO>> GetByUser(Guid cutomerId);
        Task<List<OrderDTO>> GetAll();
        Task Reject(Guid orderId);
    }
}
