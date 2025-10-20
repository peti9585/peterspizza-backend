namespace PetersPizza.Api.Infrastructure.DataTransferObjects.Pizza;

public class DbPizzaById
{
    public int Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public decimal Price { get; init; }
}