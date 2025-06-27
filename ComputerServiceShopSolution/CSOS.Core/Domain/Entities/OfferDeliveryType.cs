using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Entities.Models
{
    public class OfferDeliveryType : BaseModel
    {
        public int OfferId { get; set; }
        public int DeliveryTypeId { get; set; }

        [ForeignKey("DeliveryTypeId")]
        public DeliveryType DeliveryType { get; set; } = null!;

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; } = null!;
    }
}
