using Microsoft.EntityFrameworkCore;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Mappers;
using OrderPaymentSystem.Orders.Application.Models.Orders;
using OrderPaymentSystem.Orders.Domain;
using OrderPaymentSystem.Orders.Domain.Entities;
using OrderPaymentSystem.Orders.Domain.Exceptions;

namespace OrderPaymentSystem.Orders.Application.Services
{
    public class OrdersService(OrdersDbContext context, ICartsService cartsService) : IOrdersService
    {
        // TODO: Вынести отсюда всю логику с маппингом в статик методы сущностей
        public async Task<OrderDTO> Create(CreateOrderDTO order)
        {
            var orderByOrderNumber = await context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == order.OrderNumber && o.MerchantId == order.MerchantId);

            if (orderByOrderNumber != null)
            {
                throw new DuplicateEntityException("Order", nameof(order.OrderNumber), order.OrderNumber, $"for merchant id '{order.MerchantId}'");
            }

            if (order.Cart == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

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

            return orderSaveResultEntity.ToDTO();
        }

        public async Task<List<OrderDTO>> GetAll()
        {
            var entity = await context.Orders
                .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                .ToListAsync();

            return entity.ConvertAll(x => x.ToDTO());
        }

        public async Task<OrderDTO> GetById(long orderId)
        {
            var entity = await context.Orders
                .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return entity == null ? throw new EntityNotFoundException("Order", "Id", orderId) : entity.ToDTO();
        }

        public async Task<List<OrderDTO>> GetByUser(long cutomerId)
        {
            var entity = await context.Orders
                .Include(o => o.Cart)
                .ThenInclude(c => c.CartItems)
                .Where(o => o.CustomerId == cutomerId)
                .ToListAsync();

            return entity.ConvertAll(x => x.ToDTO());
        }

        // TODO: Добавить статусную модель для заказов
        public Task Reject(long orderId)
        {
            throw new NotImplementedException();
        }
    }
}
