using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderPaymentSystem.Orders.Domain.Aggregates.CartAggregate;
using OrderPaymentSystem.Orders.Domain.Aggregates.MerchantAgregate;
using OrderPaymentSystem.Orders.Domain.Aggregates.OrderAggregate;
using OrderPaymentSystem.Orders.Domain.Aggregates.User;
using OrderPaymentSystem.Orders.Domain.Aggregates.UserAggregate;

namespace OrderPaymentSystem.Orders.Domain;

public sealed class OrdersDbContext : IdentityDbContext<User, IdentityRole, long>
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }
    }

    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Merchant> Merchants { get; set; } = null!;
}