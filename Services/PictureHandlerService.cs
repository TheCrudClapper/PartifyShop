using ComputerServiceOnlineShop.Abstractions;
using System.Threading.Tasks;
namespace ComputerServiceOnlineShop.Services
{
    public class PictureHandlerService : IPictureHandlerService
    {
        string response = "";
        string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        public string CheckFileExtensions(List<IFormFile> uploadedImages)
        {
            if(uploadedImages != null && uploadedImages.Count > 0)
            {
                foreach (var image in uploadedImages)
                {
                    var extension = Path.GetExtension(image.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        response = $"Images should be only in formats {string.Join(',', allowedExtensions)}";
                        return response;
                    }
                }
            }
            else
            {
                response = $"Add at least one image in extendsion {string.Join(',', allowedExtensions)}";
                return response;
            }
            return response = "OK";
        }

        public async Task<List<string>> SavePicturesToDirectory(List<IFormFile> uploadedImages)
        {
            List<string> imagePaths = new List<string>();

            string directoryPath = "wwwroot/offer-images/";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if(uploadedImages != null && uploadedImages.Count > 0)
            {
                foreach(var file in uploadedImages)
                {
                    if(file.Length > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName)
                            + "_" + Guid.NewGuid()
                            + Path.GetExtension(file.FileName).ToLower();

                        var filePath = Path.Combine(directoryPath, fileName);
                        

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        imagePaths.Add($"/offer-images/{fileName}");
                    }
                }
            }
            return imagePaths;
        }
    }
}
