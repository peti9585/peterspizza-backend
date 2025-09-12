using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace PetersPizza.Api.Application.Services.Admin;

public class AdminService(
    IWebHostEnvironment environment,
    IAdminRepository adminRepository) : IAdminService
{
    public async Task UploadPizzaAsync(UploadPizzaRequest request)
    {
        var uploadsFolder = Path.Combine(environment.WebRootPath, "images");
        var fileName = Guid.NewGuid();
        
        Directory.CreateDirectory(uploadsFolder);
        
        var filePath = Path.Combine(uploadsFolder, fileName + Path.GetExtension(request.PizzaName));

        var insertPizzaRequest = new InsertPizzaRequest
        {
            PizzaName = request.PizzaName,
            Description = request.Description,
            PizzaImageId = fileName
        };
        
        var copyTask = CompressAndSaveAsync(request.PizzaImage, filePath);
        var persistTask = adminRepository.InsertPizzaAsync(insertPizzaRequest);

        await Task.WhenAll(copyTask, persistTask);
    }
    
    private static async Task CompressAndSaveAsync(IFormFile imageFile, string outputPath, int quality = 50)
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
}