using Microsoft.AspNetCore.Http;

namespace PetersPizza.Api.ViewModels.Admin;

public class UploadPizzaRequest
{
    public string PizzaName { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public IFormFile PizzaImage { get; init; }
}