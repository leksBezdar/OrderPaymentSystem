﻿using OrderPaymentSystem.Orders.Application.Models.Carts;

namespace OrderPaymentSystem.Orders.Application.Models.Orders
{
    public class CreateOrderDTO
    {
        public string? Name { get; set; }
        public long OrderNumber { get; set; }
        public long CustomerId { get; set; }

        public CartDTO Cart { get; set; } = null!;
    }
}
