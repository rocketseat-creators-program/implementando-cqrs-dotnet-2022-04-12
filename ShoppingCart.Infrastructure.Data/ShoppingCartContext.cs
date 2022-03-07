using Microsoft.EntityFrameworkCore;

using ShoppingCart.Domain;

namespace ShoppingCart.Infrastructure.Data;

public class ShoppingCartContext : DbContext
{
    public ShoppingCartContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<UserBasket> Baskets { get; set; }
}
