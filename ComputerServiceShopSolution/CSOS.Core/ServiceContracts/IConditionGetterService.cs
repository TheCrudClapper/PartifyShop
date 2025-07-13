using CSOS.Core.DTO;

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
