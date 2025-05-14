using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.DTO.Responses.Addresses
{
    public class EditAddressResponseDto
    {
        public int Id { get; set; }
        public string Place { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string SelectedCountry { get; set; } = null!;
        public List<SelectListItemDto>? CountriesSelectionList { get; set; }
    }
}
