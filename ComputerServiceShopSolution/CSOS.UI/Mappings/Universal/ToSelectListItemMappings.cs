using CSOS.Core.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.Mappings.Universal
{
    //extension class that maps select list item dto -> select list item
    public static class ToSelectListItemMappings
    {
        public static List<SelectListItem> ToSelectListItem(this IEnumerable<SelectListItemDto>? items)
        {
            if (items == null)
                return new List<SelectListItem>();

            return items.Select(item => new SelectListItem
            {
                Text = item.Text,
                Value = item.Value,
            })
            .ToList();
        } 
    }
}
