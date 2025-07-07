using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using CSOS.Core.DTO.Responses.Offers;
using CSOS.UI.Helpers.Contracts;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class MainPageCardMappings
    {
        public static MainPageCardViewModel ToViewModel(this MainPageCardResponseDto dto, IConfigurationReader configurationReader)
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

        public static IEnumerable<MainPageCardViewModel> ToViewModel(this IEnumerable<MainPageCardResponseDto> dtos, IConfigurationReader configurationReader)
        {
            return dtos.Select(dto => dto.ToViewModel(configurationReader));
        }
    }
}
