using CSOS.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.ViewModels.OfferViewModels
{
    public class OfferBrowserViewModel
    {
        public List<OfferIndexItemViewModel> Items = new List<OfferIndexItemViewModel>();
        public OfferFilter Filter { get; set; } = null!;

        public List<SelectListItem> DeliveryOptions = new List<SelectListItem>();

        public List<SelectListItem> SortingOptions = new List<SelectListItem>();
    }
}
