using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;
using CSOS.Core.DTO.Responses.Deliveries;
using CSOS.Core.DTO.Responses.Offers;

namespace CSOS.Core.Mappings.ToDto
{
    public static class OfferMappings
    {

        private const string DefaultImagePath = "wwwroot/images/no-image/png";

        public static List<MainPageCardResponseDto> ToListMainPageCardDto(this List<Offer> offers)
        {
            return offers.Select(item =>
            {
                var activeImage = item.Product.ProductImages.FirstOrDefault(img => img.IsActive);
                return new MainPageCardResponseDto()
                {
                    Id = item.Id,
                    ImagePath = activeImage?.ImagePath ?? DefaultImagePath,
                    Price = item.Price,
                    Title = item.Product.ProductName
                };

            })
            .ToList();
        }

        public static OfferResponseDto ToOfferResponseDto(this Offer offer)
        {
            return new OfferResponseDto()
            {
                Id = offer.Id,
                Condition = offer.Product.Condition.ConditionTitle,
                DateCreated = offer.DateCreated.Date,
                Seller = offer.Seller.UserName!,
                Description = offer.Product.Description,
                Price = offer.Price,
                StockQuantity = offer.StockQuantity,
                Title = offer.Product.ProductName,
                Place = offer.Seller.Address.Place,
                PostalCity = offer.Seller.Address.PostalCity,
                PostalCode = offer.Seller.Address.PostalCode,
                ProductImages = offer.Product.ProductImages
                        .Where(item => item.IsActive)
                        .Select(item => item.ImagePath)
                        .ToList(),
                AvaliableDeliveryTypes = offer.OfferDeliveryTypes
                    .Select(item => new DeliveryTypeResponseDto()
                    {
                        Title = item.DeliveryType.Title,
                        Price = item.DeliveryType.Price,
                        Id = item.DeliveryType.Id
                    }).ToList()
            };
        }

        public static List<OfferBrowserItemResponseDto> ToListOfferItemBrowserResponseDto(this List<Offer> offers)
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
                    ImageUrl = activeImage?.ImagePath ?? DefaultImagePath
                };

            })
            .ToList();
        }

        public static List<UserOffersResponseDto> ToListUserOffersResponseDto(this List<Offer> offers)
        {
            return offers.Select(item =>
            {
                var activeImage = item.Product.ProductImages.FirstOrDefault(img => img.IsActive);

                return new UserOffersResponseDto()
                {
                    Id = item.Id,
                    DateCreated = item.DateCreated,
                    ProductCondition = item.Product.Condition.ConditionTitle,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    ProductCategory = item.Product.ProductCategory.Name,
                    ProductStatus = item.IsOfferPrivate,
                    ProductName = item.Product.ProductName,
                    ImageUrl = activeImage?.ImagePath ?? DefaultImagePath
                };
            })
           .ToList();
        }

        public static EditOfferResponseDto ToEditOfferResponseDto(this Offer offer)
        {
            return new EditOfferResponseDto()
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
