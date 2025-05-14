using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Entities.Models
{
    public class DeliveryType : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Description{ get; set; } = null!;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public ICollection<OfferDeliveryType> OfferDeliveryTypes { get; set; } = new List<OfferDeliveryType>();
    }
}
