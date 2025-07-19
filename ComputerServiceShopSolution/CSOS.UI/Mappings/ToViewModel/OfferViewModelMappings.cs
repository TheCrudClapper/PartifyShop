using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.OfferDto;
using CSOS.UI.Mappings.Universal;
using CSOS.UI.ViewModels.OfferViewModels;
using CSOS.UI.ViewModels.SharedViewModels;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class OfferViewModelMappings
    {
        public static EditOfferViewModel ToEditOfferViewModel(this EditOfferResponse dto)
        {
            return new EditOfferViewModel
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
        
        
        public static OfferBrowserViewModel ToOfferIndexViewModel(this OfferIndexResponse dto, IConfigurationReader configurationReader)
        {
            return new OfferBrowserViewModel
            {
                Filter = dto.Filter,
                SortingOptions = dto.SortingOptions.ToSelectListItem(),
                DeliveryOptions = dto.DeliveryOptions.ToSelectListItem(),
                Items = dto.Items.Select(item => new OfferIndexItemViewModel()
                {
                    ProductCategory = item.ProductCategory,
                    ProductCondition = item.ProductCondition,
                    DateCreated = item.DateCreated,
                    Description = item.Description,
                    Id = item.Id,
                    ImageUrl = string.IsNullOrEmpty(item.ImageUrl) ? configurationReader.DefaultProductImage : item.ImageUrl,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    Seller = item.Seller,
                    ProductName = item.ProductName,
                }).ToList(),
            };
        }

        public static OfferDetailsViewModel ToOfferDetailsViewModel(this OfferResponse dto, IConfigurationReader configurationReader)
        {
            
            return new OfferDetailsViewModel
            {
                AvaliableDeliveryTypes = dto.AvaliableDeliveryTypes.Select(item => new DeliveryTypeViewModel
                {
                    Price = item.Price,
                    Title = item.Title,
                    Id = item.Id,
                }).ToList(),
                ProductCondition = dto.ProductCondition,
                Id = dto.Id,
                Title = dto.Title,
                Price = dto.Price,
                DateCreated = dto.DateCreated,
                Description = dto.Description,
                Place = dto.Place,
                PostalCity = dto.PostalCity,
                PostalCode = dto.PostalCode,
                Seller= dto.Seller,
                ProdcutCategory = dto.ProductCategory,
                StockQuantity = dto.StockQuantity,
                ProductImages = dto.ProductImages.Select(img => string.IsNullOrEmpty(img) ? configurationReader.DefaultProductImage : img).ToList(),
            };
        }

        public static UserOffersViewModel ToUserOffersViewModel(this UserOfferResponse dto, IConfigurationReader configurationReader)
        {
            return new UserOffersViewModel
            {
                DateCreated = dto.DateCreated,
                Id = dto.Id,
                ImageUrl = string.IsNullOrEmpty(dto.ImageUrl) ? configurationReader.DefaultProductImage : dto.ImageUrl,
                Price = dto.Price,
                ProductCategory = dto.ProductCategory,
                ProductCondition = dto.ProductCondition,
                ProductName = dto.ProductName,
                IsOfferPrivate = dto.IsOfferPrivate,
                StockQuantity = dto.StockQuantity,
            };
        }
    }
}
