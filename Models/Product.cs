using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Models
{
    public class Product : BaseModel
    {
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImagePath { get; set; } = null!;
        public int ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId ")]
        public ProductCategory ProductCategory { get; set; } = null!;
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
