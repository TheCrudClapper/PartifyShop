using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ComputerServiceOnlineShop.ViewModels.AddressViewModels
{
    public class EditAddressViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Place is required")]
        public string Place { get; set; } = null!;

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "House Number is required")]
        public string HouseNumber { get; set; } = null!;

        [Required(ErrorMessage = "Postal City is required")]
        public string PostalCity { get; set; } = null!;

        [Required(ErrorMessage = "Postal Code is required")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; } = null!;

        [Required(ErrorMessage = "Country is required")]
        public string SelectedCountry { get; set; } = null!;
        public List<SelectListItem>? CountriesSelectionList { get; set; }
    }
}
