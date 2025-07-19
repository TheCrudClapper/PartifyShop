using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;

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
