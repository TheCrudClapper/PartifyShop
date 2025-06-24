using ComputerServiceOnlineShop.Services;
using ComputerServiceOnlineShop.ViewModels.OfferViewModels;
using CSOS.Core.Domain.ExternalServicesContracts;
using CSOS.Core.DTO;
using System.Threading.Tasks;

namespace CSOS.UI.Mappings.ToDto
{
    public static  class AddOrderDtoMapping
    {
        public static AddOfferDto ToAddOfferDto(this AddOfferViewModel viewModel)
        {
            return new AddOfferDto()
            {
                Description = viewModel.Description,
                IsOfferPrivate = viewModel.IsOfferPrivate,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                SelectedOtherDeliveries = viewModel.SelectedOtherDeliveries,
                SelectedParcelLocker = viewModel.SelectedParcelLocker,
                UploadedImages = viewModel.UploadedImages,
                SelectedProductCategory = int.Parse(viewModel.SelectedProductCategory),
                SelectedProductCondition = int.Parse(viewModel.SelectedProductCondition),
                StockQuantity = viewModel.StockQuantity,
            };
        }
    }
}
