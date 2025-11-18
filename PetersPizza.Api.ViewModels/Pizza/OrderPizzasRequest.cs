namespace PetersPizza.Api.ViewModels.Pizza;

public class OrderPizzasRequest
{
    public int UserId { get; init; }

    public Guid OrderId { get; init; } = Guid.NewGuid();
    
    public IEnumerable<OrderPizzaRequest> OrderPizzaRequests { get; init; }
}