namespace PetersPizza.Api.ViewModels.Pizza;

public class GetPizzasByIdsResponse
{
    public IEnumerable<GetPizzaByIdResponse> GetPizzaResponses { get; init; }
}