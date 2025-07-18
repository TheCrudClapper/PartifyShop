namespace CSOS.Core.DTO.OfferDto
{
    public class OfferBrowserItemResponseDto
    {
        public int Id { get; set; }
        public string Condition { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Description { get; set; } = null!;
        public int QuantityAvailable { get; set; }
        public decimal Price { get; set; }
        public string SellerName { get; set; } = null!;
    }
}
