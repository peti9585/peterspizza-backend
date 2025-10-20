namespace PetersPizza.Api.Models.Pizza;

public class GetAllPizzasResponse
{
    public IEnumerable<GetPizzaResponse> GetAllPizzasResponses { get; init; }
}