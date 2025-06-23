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

        [HttpGet]
        public async Task<IActionResult> AddOrder()
        {
            var response = await _addressService.GetUserAddresInfo();

            if(response.IsFailure)
                return View("Error", response.Error.Description);


            var viewModel = new AddOrderViewModel()
            {
                UserAddressDetails = response.Value.ToViewModel()
            };
            return View(viewModel);
        }
    }
}
