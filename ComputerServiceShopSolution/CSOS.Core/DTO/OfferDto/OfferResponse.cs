using CSOS.Core.DTO.DeliveryTypeDto;

namespace CSOS.Core.DTO.OfferDto
{
    public class OfferResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Condition { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Category { get; set; } = null!;
        public bool IsOfferPrivate { get; set; } 
        public bool IsSellerCompany { get; set; }
        public string Seller { get; set; } = null!;
        public int StockQuantity { get; set; }
        
        public string ImageUrl { get; set; } = null!;
        public int Quantity { get; set; }
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Place { get; set; } = null!;
        public decimal Price { get; set; }

        public List<string> ProductImages = new List<string>();

        public List<DeliveryTypeResponse> AvaliableDeliveryTypes = new List<DeliveryTypeResponse>();
    }
}
