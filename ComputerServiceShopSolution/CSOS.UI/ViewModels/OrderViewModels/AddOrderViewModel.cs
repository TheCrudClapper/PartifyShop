using ComputerServiceOnlineShop.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ComputerServiceOnlineShop.ViewModels.OrderViewModels
{
    public class AddOrderViewModel
    {
        public UserAddressDetailsViewModel UserAddressDetails { get; set; } = null!;
    }
}
