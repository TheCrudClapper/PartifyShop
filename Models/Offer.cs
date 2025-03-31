using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Models
{
    public class Offer : BaseModel
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool OfferStatus { get; set; }
        public int SellerId { get; set; }
        [ForeignKey("SellerId")]
        public User Seller { get; set; } = null!;
        public ICollection<OfferDeliveryType> OfferDeliveryTypes { get; set; } = new List<OfferDeliveryType>();
        
     }
}
