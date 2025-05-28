using ComputerServiceOnlineShop.Entities.Models;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductImageRepository
    {
        Task<List<ProductImage>> GetImagesFromOffer(int offerId, List<string> imageUrls);
    }
}
