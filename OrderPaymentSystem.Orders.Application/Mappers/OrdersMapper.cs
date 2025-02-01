using OrderPaymentSystem.Orders.Application.Models.Carts;
using OrderPaymentSystem.Orders.Application.Models.Orders;
using OrderPaymentSystem.Orders.Domain.Aggregates.CartAggregate;
using OrderPaymentSystem.Orders.Domain.Aggregates.OrderAggregate;

namespace OrderPaymentSystem.Orders.Application.Mappers;
public static class OrdersMapper
{
    public static OrderDTO ToDTO(this Order order, Cart? cart = null)
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

    public static Order ToEntity(this CreateOrderDTO order, CartDTO? cart = null)
    {
        return new Order()
        {
            CustomerId = order.CustomerId,
            Cart = cart?.ToEntity(),
            Name = order.Name,
            OrderNumber = order.OrderNumber
        };
    }
}
