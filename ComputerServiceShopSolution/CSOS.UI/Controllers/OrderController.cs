using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.ViewModels.OrderViewModels;
using CSOS.UI.Mappings.ToViewModel;
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
            var response = await _addressService.GetUserAddresInfo();
            var viewModel = new AddOrderViewModel()
            {
                UserAddressDetails = response.ToViewModel()
            };
            return View(viewModel);
        }
    }
}
