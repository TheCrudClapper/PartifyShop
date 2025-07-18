namespace CSOS.Core.DTO.OfferDto
{
    public class BaseOfferResponse
    {
        public int Id { get; set; }
        public string ProductCondition { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductCategory { get; set; } = null!;
        
    }
}