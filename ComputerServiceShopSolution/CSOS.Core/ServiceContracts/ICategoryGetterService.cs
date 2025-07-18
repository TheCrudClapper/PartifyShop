﻿using CSOS.Core.DTO.UniversalDto;

namespace CSOS.Core.ServiceContracts
{
    public interface ICategoryGetterService
    {
        /// <summary>
        /// Gets Product categories to display in main page of application
        /// </summary>
        /// <returns>An List of MainPageCardViewModel items</returns>
        Task<IEnumerable<CardResponse>> GetProductCategoriesAsCardResponse();
        /// <summary>
        /// Gets ProductCategories as SelectListItems
        /// </summary>
        /// <returns>An List of  all product categories as SelectListType</returns>
        Task<IEnumerable<SelectListItemDto>> GetProductCategoriesAsSelectList();

    }
}
