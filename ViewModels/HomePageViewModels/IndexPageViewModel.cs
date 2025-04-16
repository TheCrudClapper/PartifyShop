using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.ViewModels.IndexPageViewModel
{
    public class IndexPageViewModel
    {
        public IEnumerable<MainPageCardViewModel> Cards = new List<MainPageCardViewModel>();
        public IEnumerable<SelectListItem> Categories = new List<SelectListItem>();
        public string? SelectedCategory { get; set; }
    }
}
