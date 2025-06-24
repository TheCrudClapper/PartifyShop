using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerServiceOnlineShop.ViewModels.OfferViewModels
{
    /// <summary>
    /// View model for editing existing offer in shop
    /// </summary>
    public class EditOfferViewModel : BaseOfferViewModel
    {
        public int Id { get; set; }

        //Uploaded images by user
        public List<IFormFile> UploadedImages { get; set; } = null!;

        //Uploaded images by user processed to urls (already saved on drive)
        public List<string>? UploadedImagesUrls { get; set; }

        //Images taken from database
        public List<SelectListItem>? ExistingImagesUrls { get; set; } = new List<SelectListItem>();

        //Images selected by user for deletion
        public List<string>? ImagesToDelete { get; set; } = new List<string>();
    }
}
