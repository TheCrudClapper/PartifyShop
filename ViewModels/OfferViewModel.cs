using ComputerServiceOnlineShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ComputerServiceOnlineShop.ViewModels
{
    public class OfferViewModel
    {
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; } = null!;


        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;


        [Required(ErrorMessage = "Product Category is required")]
        public string SelectedProductCategory { get; set; } = null!;
        public List<SelectListItem>? ProductCategoriesSelectionList { get; set; }


        [Required(ErrorMessage = "Product Condition is required")]
        public string SelectedProductCondition { get; set; } = null!;
        public List<SelectListItem>? ProductConditionsSelectList { get; set; }


        //Holds images uploaded by user in form
        [Required(ErrorMessage = "Please, upload at least one image")]
        public List<IFormFile> UploadedImages { get; set; } = null!;

        //Holds images processed to urls
        public List<string> UploadedImagesUrls { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be at least 1 PLN")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int StockQuantity { get; set; }


       
        //Holds values selected via checkboxes, needs to be initialized bc it throws nulls - at loading via get we dont have nothing selected
        [Required(ErrorMessage = "Those deliveries are required")]
        //Holds selected deliveries (can be multiple)
        public List<int> SelectedOtherDeliveries { get; set; } = new List<int>();
        public List<SelectListItem>? OtherDeliveriesSelectedList { get; set; }


        ////Holds values about parcel lockers from database (radio buttons)
        public int? SelectedParcelLocker { get; set; }
        public List<DeliveryType>? DeliveryTypes { get; set; }
    }
}
