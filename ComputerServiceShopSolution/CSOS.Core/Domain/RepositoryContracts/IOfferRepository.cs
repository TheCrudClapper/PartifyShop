using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;
using CSOS.Core.Helpers;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IOfferRepository 
    {
        Task AddAsync(Offer entity);
        Task<Offer?> GetOfferWithDetailsToEdit(int id, Guid userId);
        Task UpdateAsync(Offer entity, int id);
        Task<Offer?> GetOfferByIdAsync(int id);
        Task<List<Offer>> GetOffersByTakeAsync(int take = 12);
        Task<List<Offer>> GetFilteredUserOffersAsync(string? title, Guid userId);
        Task<List<Offer>> GetFilteredOffersAsync(OfferFilter filter);
        Task<Offer?> GetUserOffersByIdAsync(int id, Guid userId);
        Task<Offer?> GetOfferWithAllDetailsAsync(int id);
        Task<Offer?> GetOfferWithAllDetailsByUserAsync(int id, Guid userId);
        Task<bool> IsOfferInDb(int id);
        Task<List<SelectListItemDto>> GetOfferPicturesAsSelectListDto(int id);
        Task<int> GetNonPrivateOfferCount();
    }
}
