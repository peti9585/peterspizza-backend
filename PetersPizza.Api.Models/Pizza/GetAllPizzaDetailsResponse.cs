namespace PetersPizza.Api.Models.Pizza;

public class GetAllPizzaDetailsResponse
{
    public IEnumerable<GetAllPizzaDetailResponse> GetAllPizzaDetailResponses { get; init; }
}