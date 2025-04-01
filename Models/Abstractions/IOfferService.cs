using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.Models.Abstractions
{
    public interface IOfferService
    {
        Task Add(OfferViewModel model);
        Task<List<SelectListItem>> GetProductCategories();
        Task<List<SelectListItem>> GetProductConditions();
        Task<List<DeliveryType>> GetDeliveryTypes();
        Task<List<SelectListItem>> GetOtherDeliveryTypes();
        Task<List<DeliveryType>> GetParcelLockerDeliveryTypes();
    }
}
