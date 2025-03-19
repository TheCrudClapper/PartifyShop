using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Models
{
    public class CartItem : BaseModel
    {
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = null!;

        public int OfferId { get; set; }
        [ForeignKey("OfferId")]
        public Offer Offer { get; set; } = null!;
        
        public int Quantity { get; set; }
    }
}
