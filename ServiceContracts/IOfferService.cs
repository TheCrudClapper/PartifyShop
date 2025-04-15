using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ServiceContracts.DTO;
using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using ComputerServiceOnlineShop.ViewModels.SharedViewModels;
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
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Edit(EditOfferDto dto);
        /// <summary>
        /// Method that gets offer of specific id from database
        /// </summary>
        /// <param name="id">Id of offer</param>
        /// <returns>Returns an element from database</returns>
        Task<SingleOfferViewModel> ShowOffer(int id);
        /// <summary>
        /// Deletes user's offer from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteOffer(int id);
        Task<EditOfferViewModel> GetOfferForEdit(int id);
        Task DeleteImagesFromOffer(int offerId, List<string> imageUrls);
        Task<List<SelectListItem>> GetOfferPictures(int id);
        Task<bool> DoesOfferExist(int id);
        Task<IEnumerable<MainPageCardViewModel>> GetIndexPageOffers();
        Task<IEnumerable<OfferBrowserViewModel>> GetAllOffers();
        Task<IEnumerable<UserOffersViewModel>> GetUserOffers();
        Task<List<SelectListItem>> GetProductCategories();
        Task<List<SelectListItem>> GetProductConditions();
        Task<List<SelectListItem>> GetOtherDeliveryTypes();
        Task<List<DeliveryTypeViewModel>> GetParcelLockerDeliveryTypes();
    }
}
