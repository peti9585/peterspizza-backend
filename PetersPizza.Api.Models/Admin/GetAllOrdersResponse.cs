namespace PetersPizza.Api.Models.Admin;

public class GetAllOrdersResponse
{
    public IEnumerable<GetAllOrderResponse> GetAllOrderResponses { get; init; }
}