using ComputerServiceOnlineShop.Abstractions;
using ComputerServiceOnlineShop.Services;
using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.DTO;
using System.Threading.Tasks;

namespace CSOS.UI.Mappings.ToDto
{
    public static  class AddOrderDtoMapping
    {
        public static async Task<AddOfferDto> ToDto(this AddOfferViewModel viewModel, IPictureHandlerService pictureHandlerService)
        {
            return new AddOfferDto()
            {
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.IsOfferPrivate,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                StockQuantity = viewModel.StockQuantity,
                UploadedImagesUrls = viewModel.UploadedImages != null && viewModel.UploadedImages.Count > 0
                    ? await pictureHandlerService.SavePicturesToDirectory(viewModel.UploadedImages)
                    : new List<string>()
            };
        }
    }
}
