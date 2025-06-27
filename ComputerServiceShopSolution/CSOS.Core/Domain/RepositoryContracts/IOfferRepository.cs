using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.Helpers;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IOfferRepository 
    {
        Task AddAsync(Offer entity);
        Task<Offer?> GetOfferWithDetailsToEditAsync(int id, Guid userId);
        Task<Offer?> GetOfferByIdAsync(int id);
        Task<List<Offer>> GetOffersByTakeAsync(int take = 12);
        Task<List<Offer>> GetFilteredUserOffersAsync(string? title, Guid userId);
        Task<List<Offer>> GetFilteredOffersAsync(OfferFilter filter);
        Task<Offer?> GetUserOffersByIdAsync(int id, Guid userId);
        Task<Offer?> GetOfferWithAllDetailsAsync(int id);
        Task<Offer?> GetOfferWithAllDetailsByUserAsync(int id, Guid userId);
        Task<bool> IsOfferInDbAsync(int id);
        Task<int> GetNonPrivateOfferCount();
    }
}
