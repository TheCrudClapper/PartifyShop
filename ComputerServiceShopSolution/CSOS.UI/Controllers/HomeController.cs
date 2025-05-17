using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using CSOS.UI.Mappings.ToViewModel;
using CSOS.UI.Mappings.Universal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace ComputerServiceOnlineShop.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IOfferService _offerService;

    public HomeController(IOfferService offerService)
    {
        _offerService = offerService;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new IndexPageViewModel()
        {
            Cards = (await _offerService.GetIndexPageOffers()).ToViewModel(),
            Categories = (await _offerService.GetProductCategoriesAsSelectList()).ConvertToSelectListItem(),
            CategoriesSlider = (await _offerService.GetProductCategories()).ToViewModel(),
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
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
