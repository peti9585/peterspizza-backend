namespace PetersPizza.Api.Models.Pizza;

public class GetPizzasByIdsResponse
{
    public IEnumerable<GetPizzaByIdResponse> GetPizzaResponses { get; init; }
}