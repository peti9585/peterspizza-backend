namespace PetersPizza.Api.ViewModels.Admin;

public class GetAllOrdersResponse
{
    public IEnumerable<GetAllOrderResponse> GetAllOrderResponses { get; init; }
}