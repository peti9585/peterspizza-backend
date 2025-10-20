namespace PetersPizza.Api.Models.Admin;

public class InsertPizzaRequest
{
    public string PizzaName { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public decimal PizzaPrice { get; init; }
    
    public Guid PizzaImageId { get; init; }
}