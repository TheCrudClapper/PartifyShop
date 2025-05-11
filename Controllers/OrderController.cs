using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAddressService _addressService;
        public OrderController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        public async Task<IActionResult> AddOrder()
        {
            var userAddressDetails = await _addressService.GetUserAddresInfo();
            var viewModel = new AddOrderViewModel()
            {
                UserAddressDetails = userAddressDetails
            };
            return View(viewModel);
        }
    }
}
