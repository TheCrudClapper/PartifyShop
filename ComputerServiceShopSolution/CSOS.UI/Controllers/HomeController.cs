using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly IConfigurationReader _configurationReader;

        public HomeController(IOfferService offerService, ICategoryGetterService categoryGetterService, IConfigurationReader configurationReader)
        {
            _offerService = offerService;
            _categoryGetterService = categoryGetterService;
            _configurationReader = configurationReader;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexPageViewModel()
            {
                Cards = (await _offerService.GetIndexPageOffers()).Select((item => item.ToMainPageCardViewModel(_configurationReader))),
                Categories = (await _categoryGetterService.GetProductCategoriesAsSelectList()).ToSelectListItem(),
                CategoriesSlider = (await _categoryGetterService.GetProductCategoriesAsMainPageCardResponseDto()).Select(item => item.ToMainPageCardViewModel(_configurationReader)),
                BestDeals = (await _offerService.GetDealsOfTheDay()).Select(item=>item.ToMainPageCardViewModel(_configurationReader)),
            };
            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            IExceptionHandlerFeature? handler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (handler != null && handler.Error != null)
                ViewBag.Error = handler.Error.Message;

            return View();
        }
    }
}


