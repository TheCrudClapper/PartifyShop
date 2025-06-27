using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Entities.Models
{
    public class Product : BaseModel
    {
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ProductCategoryId { get; set; }
        public int ConditionId { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; } = null!;

        [ForeignKey("ConditionId")]
        public Condition Condition { get; set; } = null!;
    }
}
