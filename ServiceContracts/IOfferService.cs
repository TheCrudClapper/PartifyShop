using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ServiceContracts.DTO;
using ComputerServiceOnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.Abstractions
{
    public interface IOfferService
    {
        /// <summary>
        /// Adds user's offer to database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Add(AddOfferDto dto);
        /// <summary>
        /// Edit user offer and saves changes in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Edit(AddOfferViewModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id of offer</param>
        /// <returns>Returns an element from database</returns>
        Task<SingleOfferViewModel> GetOffer(int id);
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
        Task<IEnumerable<OfferBrowserViewModel>> GetAllOffers();
        Task<IEnumerable<UserOffersViewModel>> GetUserOffers();
        Task<List<SelectListItem>> GetProductCategories();
        Task<List<SelectListItem>> GetProductConditions();
        Task<List<DeliveryType>> GetDeliveryTypes();
        Task<List<SelectListItem>> GetOtherDeliveryTypes();
        Task<List<DeliveryType>> GetParcelLockerDeliveryTypes();
    }
}
