using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Entities.Models
{
    public class Address : BaseModel
    {
        public string Place { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; } = null!;

    }
}
