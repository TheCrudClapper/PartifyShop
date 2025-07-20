using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    [Authorize]
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
