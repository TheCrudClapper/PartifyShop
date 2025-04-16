namespace ComputerServiceOnlineShop.ViewModels.OfferViewModels
{
    public class OfferFilter
    {
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string DeliveryOption { get; set; }
        public string SortOption { get; set; }
    }
}
