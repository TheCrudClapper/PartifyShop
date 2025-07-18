using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO;
using CSOS.Core.DTO.DeliveryTypeDto;
using CSOS.Core.DTO.OfferDto;
using CSOS.Core.DTO.Universal;

namespace CSOS.Core.Mappings.ToDto
{
    public static class OfferMappings
    {
        // Usunięty DefaultImagePath

        public static IEnumerable<MainPageCardResponse> ToIEnumerableMainPageCardDto(this IEnumerable<Offer> offers)
        {
            return offers.Select(item =>
            {
                var activeImage = item.Product.ProductImages.FirstOrDefault(img => img.IsActive);
                return new MainPageCardResponse()
                {
                    Id = item.Id,
                    ImageUrl = activeImage?.ImagePath,
                    Price = item.Price,
                    Title = item.Product.ProductName
                };

            })
            .ToList();
        }

        public static OfferResponse ToOfferResponse(this Offer offer)
        {
            return new OfferResponse()
            {
                Id = offer.Id,
                Condition = offer.Product.Condition.ConditionTitle,
                DateCreated = offer.DateCreated.Date,
                Seller = offer.Seller.UserName!,
                Description = offer.Product.Description,
                Category = offer.Product.ProductCategory.Name,
                Price = offer.Price,
                StockQuantity = offer.StockQuantity,
                IsSellerCompany = offer.Seller.NIP != null,
                Title = offer.Product.ProductName,
                Place = offer.Seller.Address.Place,
                PostalCity = offer.Seller.Address.PostalCity,
                PostalCode = offer.Seller.Address.PostalCode,
                ProductImages = offer.Product.ProductImages
                        .Where(item => item.IsActive)
                        .Select(item => item.ImagePath)
                        .ToList(),
                AvaliableDeliveryTypes = offer.OfferDeliveryTypes
                    .Select(item => new DeliveryTypeResponse()
                    {
                        Title = item.DeliveryType.Title,
                        Price = item.DeliveryType.Price,
                        Id = item.DeliveryType.Id
                    }).ToList()
            };
        }

        public static IEnumerable<OfferBrowserItemResponseDto> ToListOfferItemBrowserResponseDto(this IEnumerable<Offer> offers)
        {
            return offers.Select(item =>
            {
                var activeImage = item.Product.ProductImages.FirstOrDefault(img => img.IsActive);
                return new OfferBrowserItemResponseDto()
                {
                    Id = item.Id,
                    Title = item.Product.ProductName,
                    Category = item.Product.ProductCategory.Name,
                    Condition = item.Product.Condition.ConditionTitle,
                    DateCreated = item.DateCreated.Date,
                    Price = item.Price,
                    SellerName = item.Seller.UserName!,
                    Description = item.Product.Description,
                    QuantityAvailable = item.StockQuantity,
                    ImageUrl = activeImage?.ImagePath
                };

            })
            .ToList();
        }

        public static IEnumerable<UserOfferResponse> ToIEnumerableUserOffersResponseDto(this IEnumerable<Offer> offers)
        {
            return offers.Select(item =>
            {
                var activeImage = item.Product.ProductImages.FirstOrDefault(img => img.IsActive);

                return new UserOfferResponse()
                {
                    Id = item.Id,
                    DateCreated = item.DateCreated,
                    ProductCondition = item.Product.Condition.ConditionTitle,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    ProductCategory = item.Product.ProductCategory.Name,
                    IsOfferPrivate = item.IsOfferPrivate,
                    ProductName = item.Product.ProductName,
                    ImageUrl = activeImage?.ImagePath 
                };
            })
           .ToList();
        }

        // public static IEnumerable<OfferResponse> ToIEnumerableOfferResponse(this IEnumerable<Offer> offers)
        // {
        //     
        // }
        public static EditOfferResponse ToEditOfferResponseDto(this Offer offer)
        {
            return new EditOfferResponse()
            {
                Id = offer.Id,
                ProductName = offer.Product.ProductName,
                Price = offer.Price,
                ExistingImagesUrls = offer.Product.ProductImages
                  .Where(item => item.IsActive)
                  .Select(item => new SelectListItemDto()
                  {
                      Value = item.ImagePath,
                      Text = item.ImagePath,
                  })
                  .ToList(),
                Description = offer.Product.Description,
                SelectedProductCondition = offer.Product.Condition.Id.ToString(),
                SelectedProductCategory = offer.Product.ProductCategory.Id.ToString(),
                IsOfferPrivate = offer.IsOfferPrivate,
                StockQuantity = offer.StockQuantity,
                SelectedOtherDeliveries = offer.OfferDeliveryTypes
                  .Where(item => !item.DeliveryType.Title.Contains("Locker"))
                  .Select(item => item.DeliveryTypeId)
                  .ToList(),
                SelectedParcelLocker = offer.OfferDeliveryTypes
                  .Where(item => item.DeliveryType.Title.Contains("Locker"))
                  .Select(item => item.DeliveryTypeId)
                  .FirstOrDefault(),
            };
        }
    }
}
