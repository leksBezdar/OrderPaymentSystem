﻿namespace OrderPaymentSystem.Orders.Domain.Common;
public abstract class Entity<TId>
{
    public TId Id { get; protected set; }
}
