using Microsoft.AspNetCore.Http;

namespace PetersPizza.Api.Application.Interfaces.Services;

public interface IImageHandlerService
{
    // Sync
    string EnsureImagesFolderExistsAndReturnPath();
    
    byte[] GetImageBytesByFileName(string fileName);
    
    // Async
    Task CompressAndSaveAsync(IFormFile imageFile, string outputPath, int quality);
}