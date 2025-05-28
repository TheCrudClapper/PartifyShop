using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceOnlineShop.Entities.Contexts
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<Country> Countries { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; } = default!;
        public DbSet<CartItem> CartItems { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Offer> Offers { get; set; } = default!;
        public DbSet<Condition> Conditions { get; set; } = default!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = default!;
        public DbSet<DeliveryType> DeliveryTypes { get; set; } = default!;
        public DbSet<OfferDeliveryType> OfferDeliveryTypes { get; set; } = default!;
        public DbSet<ProductImage> ProductImages { get; set; } = default!;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DatabaseContext()
        {
        }
    }
}
