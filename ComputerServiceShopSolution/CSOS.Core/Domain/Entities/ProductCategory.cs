namespace ComputerServiceOnlineShop.Entities.Models
{
    public class ProductCategory : BaseModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CategoryImage { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
