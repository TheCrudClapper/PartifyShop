using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.AccountDto;

namespace CSOS.Core.Mappings.ToDomainEntity.AddressMappings
{
    public static class AddressMappings
    {
        public static Address ToAddressEntity(this RegisterRequest request)
        {
            return new Address()
            {
                Place = request.Place,
                Street = request.Street,
                PostalCity = request.PostalCity,
                PostalCode = request.PostalCode,
                HouseNumber = request.HouseNumber,
                CountryId = request.SelectedCountry,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
            };
        }
    }
}
