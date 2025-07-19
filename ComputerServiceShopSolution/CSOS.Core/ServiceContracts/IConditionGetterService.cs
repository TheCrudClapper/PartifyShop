using CSOS.Core.DTO;
using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.ServiceContracts
{
    public interface IConditionGetterService
    {
        /// <summary>
        /// Gets Product Conditions as SelectListItem object
        /// </summary>
        /// <returns>An List of SelectListItem</returns>
        Task<IEnumerable<SelectListItemDto>> GetProductConditionsAsSelectList();
    }
}
