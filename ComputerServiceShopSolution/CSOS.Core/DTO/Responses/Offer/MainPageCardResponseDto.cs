namespace CSOS.Core.DTO.Responses.Offers
{
    public class MainPageCardResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
