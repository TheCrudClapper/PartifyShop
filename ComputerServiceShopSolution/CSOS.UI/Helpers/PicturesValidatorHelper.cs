using CSOS.Core.Domain.ExternalServicesContracts;

namespace CSOS.UI.Helpers
{
    public class PicturesValidatorHelper
    {
        public static string? ValidatePictureExtensions(List<IFormFile> pictures, IPictureHandlerService service)
        {
            if (pictures == null || pictures.Count == 0)
                return null;

            string response = service.CheckFileExtensions(pictures);

            return response == "OK" ? null : response;
        }
    }
}
