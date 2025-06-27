using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductImageRepository
    {
        /// <summary>
        /// Gets an list of all images from specific offer
        /// </summary>
        /// <param name="offerId">Offer id to get images from</param>
        /// <returns>Returns an list of ProductImage Domain Models</returns>
        Task<List<ProductImage>> GetImagesFromOfferAsync(int offerId);
    }
}
