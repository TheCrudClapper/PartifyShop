using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Offers;

namespace CSOS.Core.ServiceContracts
{
    public interface ICategoryGetterService
    {
        /// <summary>
        /// Gets Product categories to display in main page of application
        /// </summary>
        /// <returns>An List of MainPageCardViewModel items</returns>
        Task<List<MainPageCardResponseDto>> GetProductCategories();
        /// <summary>
        /// Gets ProductCategories as SelectListItems
        /// </summary>
        /// <returns>An List of  all product categories as SelectListType</returns>
        Task<List<SelectListItemDto>> GetProductCategoriesAsSelectList();

    }
}
