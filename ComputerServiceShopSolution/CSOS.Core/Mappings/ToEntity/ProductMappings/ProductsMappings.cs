﻿using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;

namespace CSOS.Core.Mappings.ToEntity.ProductMappings
{
    public static class ProductsMappings
    {
        public static Product ToProductEntity(this AddOfferDto dto, Offer offer)
        {
            return new Product
            {
                Offer = offer,
                ProductName = dto.ProductName,
                Description = dto.Description,
                ConditionId = dto.SelectedProductCondition,
                ProductCategoryId = dto.SelectedProductCategory,
                IsActive = true,
                DateCreated = DateTime.Now,

                ProductImages = dto.UploadedImagesUrls != null
                ? dto.UploadedImagesUrls.Select(imageUrl => new ProductImage()
                {
                    DateCreated = DateTime.Now,
                    ImagePath = imageUrl,
                    IsActive = true
                }).ToList()
                : new List<ProductImage>()
            };
        }
    }
}
