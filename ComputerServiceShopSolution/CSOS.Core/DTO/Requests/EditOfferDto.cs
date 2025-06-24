using Microsoft.AspNetCore.Http;

namespace CSOS.Core.DTO
{
    public class EditOfferDto
    {
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int SelectedProductCategory { get; set; }
        public bool IsOfferPrivate { get; set; }
        public int SelectedProductCondition { get; set; }
        public List<IFormFile> UploadedImages { get; set; } = null!;
        public List<string>? UploadedImagesUrls { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<int> SelectedOtherDeliveries { get; set; } = null!;
        public List<string>? ImagesToDelete { get; set; } = new List<string>();
        public int? SelectedParcelLocker { get; set; }
    }
}
