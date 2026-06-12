using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Application.Services.Admin;

public class AdminService(IImageHandlerService imageHandlerService, IAdminRepository adminRepository) : IAdminService
{
    public async Task UploadPizzaAsync(UploadPizzaRequest request)
    {
        var uploadsFolder = imageHandlerService.EnsureImagesFolderExistsAndReturnPath();

        var fileName = Guid.NewGuid();
        var filePath = Path.Combine(uploadsFolder, fileName + Path.GetExtension(request.PizzaName));

        var insertPizzaRequest = new InsertPizzaRequest
        {
            PizzaName = request.PizzaName,
            Description = request.Description,
            PizzaPrice = request.PizzaPrice,
            PizzaImageId = fileName
        };
        
        var copyTask = imageHandlerService.CompressAndSaveAsync(request.PizzaImage, filePath, 50);
        var persistTask = adminRepository.InsertPizzaAsync(insertPizzaRequest);

        await Task.WhenAll(copyTask, persistTask);
    }
}