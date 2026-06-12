namespace PetersPizza.Api.Models.Pizza;

public class GetAllOrdersResponse
{
    public IEnumerable<GetAllOrderResponse> GetAllOrderResponses { get; init; }
}