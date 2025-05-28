using CSOS.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.ViewModels.OfferViewModels
{
    public class OfferBrowserViewModel
    {
        public List<OfferBrowserItemViewModel> Items = new List<OfferBrowserItemViewModel>();
        public OfferFilter Filter { get; set; }

        public List<SelectListItem> DeliveryOptions = new List<SelectListItem>();

        public List<SelectListItem> SortingOptions = new List<SelectListItem>();
    }
}
