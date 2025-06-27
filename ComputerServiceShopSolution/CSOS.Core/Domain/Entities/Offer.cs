using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ComputerServiceOnlineShop.Entities.Models
{
    public class Offer : BaseModel
    {
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsOfferPrivate { get; set; }
        public Guid SellerId { get; set; }

        [ForeignKey("SellerId")]
        public ApplicationUser Seller { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        public ICollection<OfferDeliveryType> OfferDeliveryTypes { get; set; } = new List<OfferDeliveryType>();
      
    }
}
