namespace PetersPizza.Api.Models.Pizza;

public class GetPizzaResponse
{
    public int PizzaId { get; init; }
    
    public string PizzaName { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public byte[] PizzaImageBytes { get; init; }
}