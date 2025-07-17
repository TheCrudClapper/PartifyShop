using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using CSOS.UI.ViewModels.OfferViewModels;

namespace ComputerServiceOnlineShop.ViewModels.OfferViewModels
{
    /// <summary>
    /// View model for adding new offer to shop
    /// </summary>
    public class AddOfferViewModel : BaseOfferViewModel
    {
        //Uploaded images by user
        [Required(ErrorMessage = "Please, upload at least one image")]
        public List<IFormFile> UploadedImages { get; set; } = null!;

        //Uploaded images by user processed to urls (already saved on drive)
        [BindNever]
        public List<string>? UploadedImagesUrls { get; set; }
    }
}
