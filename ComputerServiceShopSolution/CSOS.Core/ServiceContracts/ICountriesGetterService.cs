using CSOS.Core.DTO;

namespace CSOS.Core.ServiceContracts
{
    public interface ICountriesGetterService
    {   /// <summary>
        /// Method gets countries for select
        /// </summary>
        /// <returns>Returns asynchronous list item</returns>
        Task<IEnumerable<SelectListItemDto>> GetCountriesSelectionList();
    }
}
