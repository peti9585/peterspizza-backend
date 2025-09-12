using Microsoft.AspNetCore.Hosting;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Application.Services.Admin;

public class AdminService(
    IWebHostEnvironment environment,
    IAdminRepository adminRepository) : IAdminService
{
    public async Task UploadPizzaAsync(UploadPizzaRequest request)
    {
        var uploadsFolder = Path.Combine(environment.WebRootPath, "images");
        var fileName = Guid.NewGuid();
        
        var filePath = Path.Combine(uploadsFolder, fileName + Path.GetExtension(request.PizzaName));
        await using var stream = new FileStream(filePath, FileMode.Create);

        var insertPizzaRequest = new InsertPizzaRequest
        {
            PizzaName = request.PizzaName,
            Description = request.Description,
            PizzaImageId = fileName
        };
        
        var copyTask = request.PizzaImage.CopyToAsync(stream);
        var persistTask = adminRepository.InsertPizzaAsync(insertPizzaRequest);

        await Task.WhenAll(copyTask, persistTask);
        
        Directory.CreateDirectory(uploadsFolder);
    }
}