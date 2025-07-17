using CSOS.Core.DTO.Requests;
using CSOS.UI.ViewModels.OfferViewModels;

namespace CSOS.UI.Mappings.ToDto
{
    public static class OfferDtoMappings
    {
        public static OfferUpdateRequest ToEditOfferDto(this EditOfferViewModel viewModel)
        {
            return new OfferUpdateRequest()
            {
                Id = viewModel.Id,
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
