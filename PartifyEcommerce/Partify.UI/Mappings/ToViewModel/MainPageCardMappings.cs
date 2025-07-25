using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.DTO.UniversalDto;
using CSOS.UI.ViewModels.HomePageViewModels;

namespace CSOS.UI.Mappings.ToViewModel
{
    public static class MainPageCardMappings
    {
        public static MainPageCardViewModel ToMainPageCardViewModel(this CardResponse dto, IConfigurationReader configurationReader)
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
    }
}
