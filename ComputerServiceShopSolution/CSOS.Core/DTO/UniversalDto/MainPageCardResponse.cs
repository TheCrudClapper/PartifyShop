namespace CSOS.Core.DTO.Universal
{
    public class MainPageCardResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
