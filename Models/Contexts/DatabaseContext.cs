using Microsoft.EntityFrameworkCore;

namespace ComputerServiceOnlineShop.Models.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<Country> Countries { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; } = default!;
        public DbSet<CartItem> CartItems { get; set; } = default!;

        public DbSet<Product> Products { get; set; } = default!;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
    }
}
