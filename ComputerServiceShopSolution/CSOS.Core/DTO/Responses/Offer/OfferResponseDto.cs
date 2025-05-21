using CSOS.Core.DTO.Responses.Deliveries;

namespace CSOS.Core.DTO.Responses.Offers
{
    public class OfferResponseDto
    {
        public int Id { get; set; }
        public string Condition { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Title { get; set; } = null!;
        public string Seller { get; set; } = null!;
        public int StockQuantity { get; set; }
        public string Description { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Place { get; set; } = null!;
        public decimal Price { get; set; }

        public List<string> ProductImages = new List<string>();

        public List<DeliveryTypeResponseDto> AvaliableDeliveryTypes = new List<DeliveryTypeResponseDto>();
    }
}
