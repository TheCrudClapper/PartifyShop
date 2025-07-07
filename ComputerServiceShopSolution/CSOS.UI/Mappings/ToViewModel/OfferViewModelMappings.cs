using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using ComputerServiceOnlineShop.ViewModels.SharedViewModels;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.UI.Mappings.Universal;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        //OK
        public static OfferBrowserViewModel ToOfferBrowserViewModel(this OfferBrowserResponseDto dto)
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
                    ImageUrl = item.ImageUrl,
                    Price = item.Price,
                    QuantityAvailable = item.QuantityAvailable,
                    SellerName = item.SellerName,
                    Title = item.Title,
                }).ToList(),
            };
        }

        public static SingleOfferViewModel ToSingleOfferViewModel(this OfferResponseDto dto)
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
                ProductImages = dto.ProductImages,
            };
        }

        public static UserOffersViewModel ToUserOffersViewModel(this UserOffersResponseDto dto)
        {
            return new UserOffersViewModel()
            {
                DateCreated = dto.DateCreated,
                Id = dto.Id,
                ImageUrl = dto.ImageUrl ?? "",
                Price = dto.Price,
                ProductCategory = dto.ProductCategory,
                ProductCondition = dto.ProductCondition,
                ProductName = dto.ProductName,
                ProductStatus = dto.ProductStatus,
                StockQuantity = dto.StockQuantity,
            };
        }
        public static List<UserOffersViewModel> ToViewModelCollection(this List<UserOffersResponseDto> dtos)
        {
            return dtos.Select(dto => dto.ToUserOffersViewModel()).ToList();
        }

    }
}
