using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Models.Orders;
using OrderPaymentSystem.Orders.Domain;
using OrderPaymentSystem.Orders.Domain.Entities;

namespace OrderPaymentSystem.Orders.Application.Services
{
    public class OrdersService(OrdersDbContext context, ICartsService cartsService) : IOrdersService
    {
        // TODO: Вынести отсюда всю логику с маппингом в статик методы сущностей
        public async Task<OrderDTO> Create(CreateOrderDTO order)
        {
            var cart = await cartsService.Create(order.Cart);
            var orderEntity = new OrderEntity()
            {
                OrderNumber = order.OrderNumber,
                Name = order.Name,
                CustomerId = order.CustomerId,
                CartId = cart.Id
            };

            var orderSaveResultEntity = (await context.Orders.AddAsync(orderEntity)).Entity;

            await context.SaveChangesAsync();

            return new OrderDTO()
            {
                Id = orderSaveResultEntity.Id,
                CustomerId = orderSaveResultEntity.CustomerId!.Value,
                Cart = cart,
                Name = orderSaveResultEntity.Name,
                OrderNumber = orderSaveResultEntity.OrderNumber
            };
        }

        public Task<List<OrderDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> GetById(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDTO>> GetByUser(Guid cutomerId)
        {
            throw new NotImplementedException();
        }

        public Task Reject(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
