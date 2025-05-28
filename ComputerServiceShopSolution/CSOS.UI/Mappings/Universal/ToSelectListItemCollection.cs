using CSOS.Core.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.Mappings.Universal
{
    //extension class that maps select list item dto -> select list item
    public static class ToSelectListItemCollection
    {
        public static List<SelectListItem> ConvertToSelectListItem(this IEnumerable<SelectListItemDto> items)
        {
            return items.Select(item => new SelectListItem
            {
                Text = item.Text,
                Value = item.Value,
            })
            .ToList();
        } 
    }
}
