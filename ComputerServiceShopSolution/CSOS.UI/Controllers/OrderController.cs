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
        private readonly ILogger<OrderController> _logger;
        public OrderController(IAddressService addressService, ILogger<OrderController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> AddOrder()
        {
            _logger.LogInformation("Order Controller - GET AddOrder Method Called");
            var response = await _addressService.GetUserAddressDetails();

            if (response.IsFailure)
            {
                _logger.LogError("Error while fetching {UserName} address. Error {Error}.",
                    User.Identity?.Name, response.Error.Description);
                return View("Error", response.Error.Description);
            }
                

            var viewModel = new AddOrderViewModel()
            {
                UserAddressDetails = response.Value.ToUserAddressDetailsViewModel()
            };
            return View(viewModel);
        }
    }
}
