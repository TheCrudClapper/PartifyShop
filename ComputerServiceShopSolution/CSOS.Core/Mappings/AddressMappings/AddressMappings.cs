using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.AddressMappings
{
    public static class AddressMappings
    {
        public static Address ToAddressEntity(this RegisterDto dto)
        {
            return new Address()
            {
                Place = dto.Place,
                Street = dto.Street,
                PostalCity = dto.PostalCity,
                PostalCode = dto.PostalCode,
                HouseNumber = dto.HouseNumber,
                CountryId = dto.SelectedCountry,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
            };
        }
    }
}
