using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.ViewComponents
{
    public class LoadingSpinnerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
