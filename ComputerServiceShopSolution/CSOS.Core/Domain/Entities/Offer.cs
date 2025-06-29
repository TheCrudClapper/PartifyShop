using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Entities.Models
{
    /// <summary>
    /// Domain Model representing Offer in Online Shop
    /// </summary>
    public class Offer : BaseModel
    {

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsOfferPrivate { get; set; }
        public Guid SellerId { get; set; }

        [ForeignKey("SellerId")]
        public ApplicationUser Seller { get; set; } = null!;

        public Product Product { get; set; } = null!;
        public ICollection<OfferDeliveryType> OfferDeliveryTypes { get; set; } = new List<OfferDeliveryType>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
