namespace PetersPizza.Api.ViewModels.Pizza;

public class GetAllOrdersResponse
{
    public IEnumerable<GetAllOrderResponse> GetAllOrderResponses { get; init; }
}