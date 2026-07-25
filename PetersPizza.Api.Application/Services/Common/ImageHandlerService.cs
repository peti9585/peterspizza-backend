using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PetersPizza.Api.Application.Interfaces.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace PetersPizza.Api.Application.Services.Common;

// TODO: Rethink this class and its dependencies
[ExcludeFromCodeCoverage]
public class ImageHandlerService(IWebHostEnvironment environment) : IImageHandlerService
{
    public async Task CompressAndSaveAsync(IFormFile imageFile, string outputPath, int quality)
    {
        await using var inputStream = imageFile.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);

        image.Mutate(i => i.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(300, 300)
        }));
        
        var encoder = new JpegEncoder { Quality = quality };

        await image.SaveAsync(outputPath, encoder);
    }

    public string EnsureImagesFolderExistsAndReturnPath()
    {
        var uploadsFolder = Path.Combine(environment.WebRootPath, "images");
        
        Directory.CreateDirectory(uploadsFolder);
        
        return uploadsFolder;
    }

    public byte[] GetImageBytesByFileName(string fileName)
    {
        var basePath = EnsureImagesFolderExistsAndReturnPath();
        var imagePath = Path.Combine(basePath, fileName);
        var imageBytes = File.ReadAllBytes(imagePath);
        
        return imageBytes;
    }
}