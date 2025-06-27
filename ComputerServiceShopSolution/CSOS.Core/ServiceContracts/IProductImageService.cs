using CSOS.Core.DTO;
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
    }
}
