using ComputerServiceOnlineShop.Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ComputerServiceOnlineShop.ServiceContracts.DTO
{
    public class AddOfferDto
    {
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int SelectedProductCategory { get; set; }
        public bool IsOfferPrivate { get; set; }
        public int SelectedProductCondition { get; set; }
        public List<string>? UploadedImagesUrls { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<int> SelectedOtherDeliveries { get; set; } = null!;
        public int? SelectedParcelLocker { get; set; }
    }
}
