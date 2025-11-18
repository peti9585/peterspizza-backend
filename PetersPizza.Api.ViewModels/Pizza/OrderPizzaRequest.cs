namespace PetersPizza.Api.ViewModels.Pizza;

public class OrderPizzaRequest
{
    public int PizzaId { get; init; }
    
    public int Quantity { get; init; }
}