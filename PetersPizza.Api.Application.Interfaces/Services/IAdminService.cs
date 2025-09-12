using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Application.Interfaces.Services;

public interface IAdminService
{
    Task UploadPizzaAsync(UploadPizzaRequest request);
}