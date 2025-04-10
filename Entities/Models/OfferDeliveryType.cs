using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Entities.Models
{
    public class OfferDeliveryType : BaseModel
    {
        public int OfferId { get; set; }
        [ForeignKey("OfferId")]
        public Offer Offer { get; set; } = null!;

        public int DeliveryTypeId { get; set; }
        [ForeignKey("DeliveryTypeId")]
        public DeliveryType DeliveryType { get; set; } = null!;
    }
}
