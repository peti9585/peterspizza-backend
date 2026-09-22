namespace PetersPizza.Api.Infrastructure.DataAccessObjects.Admin;

public class DbGetAllOrdersForToday
{
    public int Id { get; init; }
    
    public Guid OrderId { get; init; }
    
    public string UserName { get; init; }
    
    public string PizzaName { get; init; }
    
    public decimal Price { get; init; }
    
    public int Count { get; init; }
    
    public int OrderState { get; init; }
    
    public DateTime OrderDate { get; init; }
}