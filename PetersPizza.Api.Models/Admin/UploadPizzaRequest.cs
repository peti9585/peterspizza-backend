using Microsoft.AspNetCore.Http;

namespace PetersPizza.Api.Models.Admin;

public class UploadPizzaRequest
{
    public string PizzaName { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public decimal PizzaPrice { get; init; }
    
    public IFormFile PizzaImage { get; init; }
}