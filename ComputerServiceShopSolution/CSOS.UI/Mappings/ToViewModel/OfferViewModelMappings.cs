using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using ComputerServiceOnlineShop.ViewModels.SharedViewModels;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.UI.Helpers.Contracts;
using CSOS.UI.Mappings.Universal;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class OfferViewModelMappings
    {
        public static EditOfferViewModel ToEditOfferViewModel(this EditOfferResponseDto dto)
        {
            return new EditOfferViewModel()
            {
                Description = dto.Description,
                Price = dto.Price,
                ProductName = dto.ProductName,
                Id = dto.Id,
                IsOfferPrivate = dto.IsOfferPrivate,
                SelectedOtherDeliveries = dto.SelectedOtherDeliveries,
                SelectedParcelLocker = dto.SelectedParcelLocker,
                SelectedProductCondition = dto.SelectedProductCondition,
                StockQuantity = dto.StockQuantity,
                SelectedProductCategory = dto.SelectedProductCategory,
                ExistingImagesUrls = dto.ExistingImagesUrls.ToSelectListItem()
            };
            
        }

        public static OfferBrowserViewModel ToOfferBrowserViewModel(this OfferBrowserResponseDto dto, IConfigurationReader configurationReader)
        {
            return new OfferBrowserViewModel()
            {
                Filter = dto.Filter,
                SortingOptions = dto.SortingOptions.ToSelectListItem(),
                DeliveryOptions = dto.DeliveryOptions.ToSelectListItem(),
                Items = dto.Items.Select(item => new OfferBrowserItemViewModel()
                {
                    Category = item.Category,
                    Condition = item.Condition,
                    DateCreated = item.DateCreated,
                    Description = item.Description,
                    Id = item.Id,
                    ImageUrl = string.IsNullOrEmpty(item.ImageUrl) ? configurationReader.DefaultProductImage : item.ImageUrl,
                    Price = item.Price,
                    QuantityAvailable = item.QuantityAvailable,
                    SellerName = item.SellerName,
                    Title = item.Title,
                }).ToList(),
            };
        }

        public static SingleOfferViewModel ToSingleOfferViewModel(this OfferResponseDto dto, IConfigurationReader configurationReader)
        {
            
            return new SingleOfferViewModel()
            {
                AvaliableDeliveryTypes = dto.AvaliableDeliveryTypes.Select(item => new DeliveryTypeViewModel
                {
                    Price = item.Price,
                    Title = item.Title,
                    Id = item.Id,
                }).ToList(),
                Condition = dto.Condition,
                Id = dto.Id,
                Title = dto.Title,
                Price = dto.Price,
                DateCreated = dto.DateCreated,
                Description = dto.Description,
                Place = dto.Place,
                PostalCity = dto.PostalCity,
                PostalCode = dto.PostalCode,
                Seller= dto.Seller,
                Category = dto.Category,
                StockQuantity = dto.StockQuantity,
                ProductImages = dto.ProductImages.Select(img => string.IsNullOrEmpty(img) ? configurationReader.DefaultProductImage : img).ToList(),
            };
        }

        public static UserOffersViewModel ToUserOffersViewModel(this UserOffersResponseDto dto, IConfigurationReader configurationReader)
        {
            return new UserOffersViewModel()
            {
                DateCreated = dto.DateCreated,
                Id = dto.Id,
                ImageUrl = string.IsNullOrEmpty(dto.ImageUrl) ? configurationReader.DefaultProductImage : dto.ImageUrl,
                Price = dto.Price,
                ProductCategory = dto.ProductCategory,
                ProductCondition = dto.ProductCondition,
                ProductName = dto.ProductName,
                ProductStatus = dto.ProductStatus,
                StockQuantity = dto.StockQuantity,
            };
        }
        public static List<UserOffersViewModel> ToViewModelCollection(this IEnumerable<UserOffersResponseDto> dtos, IConfigurationReader configurationReader)
        {
            return dtos.Select(dto => dto.ToUserOffersViewModel(configurationReader)).ToList();
        }

    }
}
