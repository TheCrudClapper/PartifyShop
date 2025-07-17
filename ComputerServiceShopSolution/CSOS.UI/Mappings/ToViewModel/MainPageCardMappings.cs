using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.Universal;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class MainPageCardMappings
    {
        public static MainPageCardViewModel ToViewModel(this MainPageCardResponse dto, IConfigurationReader configurationReader)
        {
            return new MainPageCardViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Price = dto.Price,
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl)
                    ? configurationReader.DefaultProductImage
                    : dto.ImageUrl,
            };
        }

        public static IEnumerable<MainPageCardViewModel> ToViewModel(this IEnumerable<MainPageCardResponse> dtos, IConfigurationReader configurationReader)
        {
            return dtos.Select(dto => dto.ToViewModel(configurationReader));
        }
    }
}
