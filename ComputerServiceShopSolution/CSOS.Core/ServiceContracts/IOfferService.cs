using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.Core.Helpers;

//put some service method to correspondign services // or create new one !!!!
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
        Task Edit(int id, EditOfferDto dto);

        /// <summary>
        /// Method that gets offer of specific id from database
        /// </summary>
        /// <param name="id">CartiD of offer</param>
        /// <returns>Returns an element from database of type SingleOfferViewModel</returns>
        Task<OfferResponseDto> GetOffer(int id);

        /// <summary>
        /// Gets user's offer with filtering by product name
        /// </summary>
        /// <param name="title">Parameter used for filtration</param>
        /// <returns>IEnumerable collection of UserOfferViewModel</returns>
        Task<List<UserOffersResponseDto>> GetFilteredUserOffers(string? title);

        /// <summary>
        /// Deletes user's offer from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteOffer(int id);

        /// <summary>
        /// Gets offer for edit, used to populate forms
        /// </summary>
        /// <param name="id">Offer id</param>
        /// <returns>Object of type EditOfferViewModel</returns>
        Task<EditOfferResponseDto> GetOfferForEdit(int id);

        /// <summary>
        /// Used with editing, deletes images from offers 
        /// </summary>
        /// <param name="offerId">Offer id</param>
        /// <param name="imageUrls">Urls of images to delete</param>
        /// <returns></returns>
        Task DeleteImagesFromOffer(int offerId, List<string> imageUrls);

        /// <summary>
        /// Gets pictures for specified offer
        /// </summary>
        /// <param name="id">Offer id</param>
        /// <returns>List of SelectListItemType</returns>
        Task<List<SelectListItemDto>> GetOfferPictures(int id);

        /// <summary>
        /// Checks wheater offer exists in database and belongs to actually logged in user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return true if offer exists, false if not</returns>
        Task<bool> DoesOfferExist(int id);

        /// <summary>
        /// Gets offer from database based on specific filter options
        /// </summary>
        /// <param name="filter">Object that consist of properties with filtration values</param>
        /// <returns>An object of OfferBrowserViewModel with fields used in view</returns>
        Task<OfferBrowserResponseDto> GetFilteredOffers(OfferFilter filter);

        /// <summary>
        /// Gets items for the main page of application
        /// </summary>
        /// <returns>An IEnumerable collection with objects of MainPageCardViewModel</returns>
        Task<List<MainPageCardResponseDto>> GetIndexPageOffers();

        /// <summary>
        /// TODO: Change this shit to have filtering options from db
        /// </summary>
        /// <returns>Returns Sorting options used in OfferBrowser Page</returns>
        List<SelectListItemDto> GetSortingOptions();

        Task<List<MainPageCardResponseDto>> GetDealsOfTheDay();
    }
}
