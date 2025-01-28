using OrderPaymentSystem.Orders.Application.Models.Carts;
using OrderPaymentSystem.Orders.Application.Models.Orders;
using OrderPaymentSystem.Orders.Domain.Entities;

namespace OrderPaymentSystem.Orders.Application.Mappers;
public static class OrdersMapper
{
    public static OrderDTO ToDTO(this OrderEntity order, CartEntity? cart = null)
    {
        return new OrderDTO()
        {
            Id = order.Id,
            CustomerId = order.CustomerId!.Value,
            Cart = cart == null ? order.Cart?.ToDTO() : cart?.ToDTO(),
            Name = order.Name,
            OrderNumber = order.OrderNumber
        };
    }

    public static OrderEntity ToEntity(this CreateOrderDTO order, CartDTO? cart = null)
    {
        return new OrderEntity()
        {
            CustomerId = order.CustomerId,
            Cart = cart?.ToEntity(),
            Name = order.Name,
            OrderNumber = order.OrderNumber
        };
    }
}
