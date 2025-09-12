namespace PetersPizza.Api.Models.Pizza;

public class GetAllPizzaDetailResponse
{
    public string PizzaName { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public Guid PizzaImageId { get; init; }
}