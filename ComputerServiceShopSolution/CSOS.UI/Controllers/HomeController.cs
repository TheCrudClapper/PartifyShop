using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using CSOS.Core.ServiceContracts;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.SharedViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace ComputerServiceOnlineShop.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IOfferService _offerService;
    private readonly ICategoryGetterService _categoryGetterService;

    public HomeController(IOfferService offerService, ICategoryGetterService categoryGetterService)
    {
        _offerService = offerService;
        _categoryGetterService = categoryGetterService;

    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new IndexPageViewModel()
        {
            Cards = (await _offerService.GetIndexPageOffers()).ToViewModel(),
            Categories = (await _categoryGetterService.GetProductCategoriesAsSelectList()).ToSelectListItem(),
            CategoriesSlider = (await _categoryGetterService.GetProductCategoriesAsMainPageCardResponseDto()).ToViewModel(),
            BestDeals = (await _offerService.GetDealsOfTheDay()).ToViewModel(),
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
    public IActionResult NewView()
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
