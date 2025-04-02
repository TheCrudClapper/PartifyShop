using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.Models.Abstractions
{
    public interface ICountriesService
    {   /// <summary>
        /// Method gets countries for select
        /// </summary>
        /// <returns>Returns asynchronous list item</returns>
        Task<List<SelectListItem>> GetCountriesSelectionList();
    }
}
