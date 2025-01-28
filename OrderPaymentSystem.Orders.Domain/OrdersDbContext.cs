﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderPaymentSystem.Orders.Domain.Entities;

namespace OrderPaymentSystem.Orders.Domain;

public sealed class OrdersDbContext : IdentityDbContext<UserEntity, IdentityRoleEntity, long>
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }
    }

    public DbSet<CustomerEntity> Customers { get; set; } = null!;
    public DbSet<CartEntity> Carts { get; set; } = null!;
    public DbSet<CartItemEntity> CartItems { get; set; } = null!;
    public DbSet<OrderEntity> Orders { get; set; } = null!;
}