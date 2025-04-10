using System.ComponentModel.DataAnnotations.Schema;
namespace ComputerServiceOnlineShop.Entities.Models
{
    /// <summary>
    /// This model represents an Cart, user can have only one cart
    /// </summary>
    public class Cart : BaseModel
    {
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalCartValue { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
