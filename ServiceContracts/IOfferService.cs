using ComputerServiceOnlineShop.Models;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.Abstractions
{
    public interface IOfferService
    {
        /// <summary>
        /// Adds user's offer to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Add(OfferViewModel model);
        /// <summary>
        /// Edit user offer and saves changes in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Edit(OfferViewModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id of offer</param>
        /// <returns>Returns an element from database</returns>
        Task<OfferViewModel> GetOffer(int id);
        /// <summary>
        /// Deletes user's offer from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteOffer(int id);
        /// <summary>
        /// Returns all avaliable offers in show
        /// </summary>
        /// <returns>An IEnumaerable collection</returns>
        Task<IEnumerable<PublicOfferViewModel>> GetAllOffers();
        Task<IEnumerable<UserOffersViewModel>> GetUserOffers();
        Task<List<SelectListItem>> GetProductCategories();
        Task<List<SelectListItem>> GetProductConditions();
        Task<List<DeliveryType>> GetDeliveryTypes();
        Task<List<SelectListItem>> GetOtherDeliveryTypes();
        Task<List<DeliveryType>> GetParcelLockerDeliveryTypes();
    }
}
