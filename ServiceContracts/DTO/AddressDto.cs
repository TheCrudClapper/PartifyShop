namespace ComputerServiceOnlineShop.ServiceContracts.DTO
{
    public class AddressDto
    {
        public string Place { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public int CountryId { get; set; }
    }
}
