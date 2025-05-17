using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.Mappings
{
    public static class ProductsMappings
    {
        public static Product ToEntity (this AddOfferDto dto)
        {
            return new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                ConditionId = dto.SelectedProductCondition,
                ProductCategoryId = dto.SelectedProductCategory,
                IsActive = true,
                DateCreated = DateTime.Now,
                ProductImages = dto.UploadedImagesUrls!.Select(imageUrl => new ProductImage()
                {
                    DateCreated = DateTime.Now,
                    ImagePath = imageUrl,
                    IsActive = true
                }).ToList()
            };
        }
    }
}
