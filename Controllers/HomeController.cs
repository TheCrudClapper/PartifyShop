using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ComputerServiceOnlineShop.Models;
using Microsoft.AspNetCore.Authorization;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Services;
using ComputerServiceOnlineShop.ViewModels;
using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
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
        var viewModel = new MainPageViewModel()
        {
            Cards = await _offerService.GetIndexPageOffers(),
        };
        return View(viewModel);
    }
    public IActionResult Privacy()
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
