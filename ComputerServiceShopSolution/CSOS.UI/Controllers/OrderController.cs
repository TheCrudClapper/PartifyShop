using ComputerServiceOnlineShop.ViewModels.OrderViewModels;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
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
            var response = await _addressService.GetUserAddressDetails();

            if(response.IsFailure)
                return View("Error", response.Error.Description);


            var viewModel = new AddOrderViewModel()
            {
                UserAddressDetails = response.Value.ToUserAddressDetailsViewModel()
            };
            return View(viewModel);
        }
    }
}
