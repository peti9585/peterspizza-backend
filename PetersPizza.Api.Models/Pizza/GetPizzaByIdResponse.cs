namespace PetersPizza.Api.Models.Pizza;

public class GetPizzaByIdResponse
{
    public int PizzaId { get; init; }
    
    public string PizzaName { get; init; } = string.Empty;
    
    public decimal PizzaPrice { get; init; }
}