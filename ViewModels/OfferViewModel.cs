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

        [Required(ErrorMessage = "Product Condition is required")]
        public string SelectedProductCondition { get; set; } = null!;

        [Required(ErrorMessage = "Please, upload at least one image")]
        public List<IFormFile> UploadedImages { get; set; } = null!;
        public Task<List<string>>? UploadedImagesUrls { get; set; }
        public List<SelectListItem>? ProductConditionsSelectList { get; set; }
        
        public List<SelectListItem>? ProductCategoriesSelectionList { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be at least 1 PLN")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int StockQuantity { get; set; }

        public List<SelectListItem>? OtherDeliveriesSelectedList{ get; set; }

        //Holds values selected via checkboxes, needs to be initialized bc it throws nulls - at loading via get we dont have nothing selected
        [Required(ErrorMessage = "Those deliveries are required")]
        public List<int> SelectedOtherDeliveries { get; set; } = new List<int>();

        //Holds vale selected via radiobox
        public int? SelectedParcelLocker { get; set; }

        //Holds values about parcel lockers from database
        public List<DeliveryType>? DeliveryTypes { get; set; }
    }
}
