using ComputerServiceOnlineShop.ViewModels.IndexPageViewModel;
using CSOS.Core.DTO.Responses.Offers;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class MainPageCardMappings
    {
        public static MainPageCardViewModel ToViewModel(this MainPageCardResponseDto dto)
        {
            return new MainPageCardViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Price = dto.Price,
                ImagePath = dto.ImagePath
            };
        }

        public static IEnumerable<MainPageCardViewModel> ToViewModel(this IEnumerable<MainPageCardResponseDto> dtos)
        {
            return dtos.Select(dto => dto.ToViewModel());
        }
    }
}
