using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSOS.UI.ViewModels.OfferViewModels
{
    /// <summary>
    /// View model for editing existing offer in shop
    /// </summary>
    public class EditOfferViewModel : BaseOfferViewModel
    {
        [Required]
        [FromRoute]
        public int Id { get; set; }

        //Uploaded images by user
        public List<IFormFile> UploadedImages { get; set; } = null!;
        
        public List<string>? UploadedImagesUrls { get; set; }

        //Images taken from database
        public List<SelectListItem>? ExistingImagesUrls { get; set; } = new List<SelectListItem>();

        //Images selected by user for deletion
        public List<string>? ImagesToDelete { get; set; } = new List<string>();
    }
}
