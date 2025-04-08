using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ComputerServiceOnlineShop.Models;
using Microsoft.AspNetCore.Authorization;
namespace ComputerServiceOnlineShop.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<CardItem> items = new List<CardItem>()
        {
            new CardItem()
            {
                Name = "Rtx 3060",
                Price = 999,
            },
            new CardItem()
            {
                Name = "Rtx 3070",
                Price = 1999,
            },
            new CardItem()
            {
                Name = "Rtx 3080",
                Price = 2999,
            },
             new CardItem()
            {
                Name = "Another card",
                Price = 2999,
            },
            new CardItem()
            {
                Name = "Even more cards",
                Price = 2999,
            },
            new CardItem()
            {
                Name = "Last Card",
                Price = 2999,
            }
        };
        return View(items);
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
