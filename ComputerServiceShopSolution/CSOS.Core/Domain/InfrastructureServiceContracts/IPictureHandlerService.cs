using Microsoft.AspNetCore.Http;

namespace CSOS.Core.Domain.ExternalServicesContracts
{
    public interface IPictureHandlerService
    {
        /// <summary>
        /// Method that saves pictures send by user in form on server.
        /// </summary>
        /// <param name="uploadedImages"></param>
        /// <returns>Returns list of disk locations to file</returns>
        Task<List<string>> SavePicturesToDirectory(List<IFormFile> uploadedImages);
        /// <summary>
        /// Method that checks whether images submitted by user are in allowed format.
        /// </summary>
        /// <param name="uploadedImages"></param>
        /// <returns>Returns potetnial error message as string</returns>
        string CheckFileExtensions(List<IFormFile> uploadedImages);
        void DeleteSelectedPictures(List<string> ImagesUrlsToDelete);
    }
}
