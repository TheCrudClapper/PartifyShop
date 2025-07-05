using CSOS.Core.DTO;

namespace ComputerServiceOnlineShop.Abstractions
{
    public interface ICountriesGetterService
    {   /// <summary>
        /// Method gets countries for select
        /// </summary>
        /// <returns>Returns asynchronous list item</returns>
        Task<List<SelectListItemDto>> GetCountriesSelectionList();
    }
}
