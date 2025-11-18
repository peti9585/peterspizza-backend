namespace PetersPizza.Api.Models.Pizza;

public class OrderPizzasRequest
{
    public int UserId { get; init; }
    
    public Guid OrderId { get; init; }
    
    public IEnumerable<OrderPizzaRequest> OrderPizzaRequests { get; init; }
}