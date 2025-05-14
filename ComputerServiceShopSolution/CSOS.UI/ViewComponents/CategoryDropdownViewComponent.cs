using ComputerServiceOnlineShop.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.ViewComponents
{
    public class CategoryDropdownViewComponent : ViewComponent
    {
        private readonly IOfferService _offerService;
        public CategoryDropdownViewComponent(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _offerService.GetProductCategoriesAsSelectList();
            return View(categories);
        }
    }
}
