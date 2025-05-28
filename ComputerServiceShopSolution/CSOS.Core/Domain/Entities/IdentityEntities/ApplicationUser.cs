using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Entities.Models.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? NIP { get; set; }
        public string? Title { get; set; }
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = null!;
        public int AdressId { get; set; }
        [ForeignKey("AdressId")]
        public Address Address { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEdited { get; set; }
        public DateTime? DateDeleted { get; set; }
        //Holds offres made by user
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();

    }
}
