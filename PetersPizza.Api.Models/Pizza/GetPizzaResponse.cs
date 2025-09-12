using Microsoft.AspNetCore.Http;

namespace PetersPizza.Api.Models.Pizza;

public class GetPizzaResponse
{
    public string PizzaName { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public byte[] PizzaImageBytes { get; init; }
}