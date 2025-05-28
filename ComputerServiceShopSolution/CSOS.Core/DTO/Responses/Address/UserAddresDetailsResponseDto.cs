using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.DTO.Responses.Addresses
{
    public class UserAddresDetailsResponseDto
    {
        public int AddressId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalInfo { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
