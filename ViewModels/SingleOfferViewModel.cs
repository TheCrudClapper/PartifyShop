using ComputerServiceOnlineShop.Entities.Models;

namespace ComputerServiceOnlineShop.ViewModels
{
    public class SingleOfferViewModel
    {
        public string Condition { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string Title { get; set; } = null!;
        public string Seller { get; set; } = null!;
        public int StockQuantity { get; set; }

        public bool isSellerCompany { get; set; }
        public string Description { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Place { get; set; } = null!;
        public decimal Price { get; set; }

        public List<string> ProductImages = new List<string>();
        public List<DeliveryTypeViewModel> AvaliableDeliveryTypes = new List<DeliveryTypeViewModel>();
    }
}
