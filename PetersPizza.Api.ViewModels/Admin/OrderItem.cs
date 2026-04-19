namespace PetersPizza.Api.ViewModels.Admin;

public class OrderItem
{
    public int OrderId { get; init; }
    
    public string PizzaName { get; init; }
    
    public int Quantity { get; init; }
    
    public decimal Price { get; init; }
}