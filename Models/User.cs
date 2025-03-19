using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Models
{
    /// <summary>
    /// This model represents an user in database
    /// </summary>
    public class User : BaseModel
    {
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        //In the future it will contain hashed password, for now it' plain text
        public string PasswordHash { get; set; } = null!;
        public string? NIP { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set;}
        public string Email { get; set; } = null!;
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = null!;
        public int AdressId { get; set; }
        [ForeignKey("AdressId")]
        public Address Address { get; set; } = null!;
        //Holds offres made by user
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
