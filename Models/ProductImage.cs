using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerServiceOnlineShop.Models
{
    public class ProductImage : BaseModel
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
    }
}
