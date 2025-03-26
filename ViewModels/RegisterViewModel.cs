using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace ComputerServiceOnlineShop.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage ="Surname is required")]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage ="Password is required")]
        [MinLength(6,ErrorMessage ="Password must have at least 6 letters")]
        public string Password { get; set; } = null!;

        public string? NIP { get; set; }

        public string? Title { get; set; }

        [MinLength(9,ErrorMessage = "Number can't have less than 9 digits")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email in not appropiate format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Palce is required")]
        public string Place { get; set; } = null!;

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "House Number is required")]
        public string HouseNumber { get; set; } = null!;

        [Required(ErrorMessage = "Postal City is required")]
        public string PostalCity { get; set; } = null!;

        [Required(ErrorMessage = "Postal Code is required")]
        public string PostalCode { get; set; } = null!;
        //country id = 3 for testing

        [Required(ErrorMessage = "Contry Code is required")]
        public int CountryId { get; set; }

        //public List<SelectListItem>? Countries { get; set; }
    }
}
