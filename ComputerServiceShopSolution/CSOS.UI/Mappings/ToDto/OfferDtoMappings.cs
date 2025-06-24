using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.DTO;

namespace CSOS.UI.Mappings.ToDto
{
    public static class OfferDtoMappings
    {
        public static EditOfferDto ToEditOfferDto(this EditOfferViewModel viewModel)
        {
            return new EditOfferDto()
            {
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.IsOfferPrivate,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                StockQuantity = viewModel.StockQuantity,
                UploadedImages = viewModel.UploadedImages,
                ImagesToDelete = viewModel.ImagesToDelete,
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
            };
        }
    }
}
