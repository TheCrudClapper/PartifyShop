namespace ComputerServiceOnlineShop.ViewModels.CartViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal? TotalCartValue { get; set; }
        public decimal? TotalDeliveryValue { get; set; }
        public decimal? TotalItemsValue{ get; set; }

    }
}
