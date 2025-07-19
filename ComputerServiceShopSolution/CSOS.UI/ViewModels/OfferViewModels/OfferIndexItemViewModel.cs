namespace CSOS.UI.ViewModels.OfferViewModels
{
    public class OfferIndexItemViewModel
    {
        public int Id { get; set; }
        public string? ProductCondition { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProductCategory { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string? Seller { get; set; }
    }
}
