namespace PetersPizza.Api.ViewModels.Pizza;

public class GetAllPizzasResponse
{
    public IEnumerable<GetPizzaResponse> GetAllPizzasResponses { get; init; }
}