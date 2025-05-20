using CSOS.Core.DTO;

namespace CSOS.Core.ServiceContracts
{
    public interface IConditionGetterService
    {
        Task<List<SelectListItemDto>> GetProductCategoriesAsSelectList();

        /// <summary>
        /// Gets Product Conditions as SelectListItem object
        /// </summary>
        /// <returns>An List of SelectListItem</returns>
        Task<List<SelectListItemDto>> GetProductConditions();
    }
}
