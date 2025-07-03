using CSOS.Core.DTO;
using CSOS.Core.ErrorHandling;
namespace CSOS.Core.ServiceContracts
{
    public interface IProductImageService
    {
        /// <summary>
        /// Gets pictures for specific offer
        /// </summary>
        /// <param name="offerId">Id of offer to fetch pictures from</param>
        /// <returns>Returns List of SelectListItemDto</returns>
        Task<List<SelectListItemDto>> GetOfferPictures(int offerId);
        /// <summary>
        /// Removes specific images from an offer.
        /// </summary>
        /// <param name="offerId">The ID of the offer.</param>
        /// <param name="imageUrls">List of image URLs to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Result> DeleteImagesFromOffer(int offerId, List<string> imageUrls);
    }
}
