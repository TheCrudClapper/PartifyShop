using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.HomePageViewModels;
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
        private readonly ILogger<HomeController> _logger;

        public HomeController(IOfferService offerService, ICategoryGetterService categoryGetterService, IConfigurationReader configurationReader, ILogger<HomeController> logger)
        {
            _offerService = offerService;
            _categoryGetterService = categoryGetterService;
            _configurationReader = configurationReader;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("HomeController - GET Index Method called");
            var viewModel = new IndexPageViewModel()
            {
                Cards = (await _offerService.GetIndexPageOffers()).Select((item => item.ToMainPageCardViewModel(_configurationReader))),
                Categories = (await _categoryGetterService.GetProductCategoriesAsSelectList()).ToSelectListItem(),
                CategoriesSlider = (await _categoryGetterService.GetProductCategoriesAsCardResponse()).Select(item => item.ToMainPageCardViewModel(_configurationReader)),
                BestDeals = (await _offerService.GetDealsOfTheDay()).Select(item=>item.ToMainPageCardViewModel(_configurationReader)),
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogInformation("HomeController - GET Error Method called");
            IExceptionHandlerFeature? handler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (handler != null && handler.Error != null)
                ViewBag.Error = handler.Error.Message;

            return View();
        }
    }
}


