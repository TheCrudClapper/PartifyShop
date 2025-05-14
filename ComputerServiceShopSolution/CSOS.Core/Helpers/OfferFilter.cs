namespace CSOS.Core.Helpers
{
    public class OfferFilter
    {
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string? DeliveryOption { get; set; }
        public string? SortOption { get; set; }
        public string? SearchPhrase { get; set; }
        public int? CategoryId { get; set; }
    }
}
