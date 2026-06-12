using PetersPizza.Api.ViewModels.Common;

namespace PetersPizza.Api.ViewModels.Pizza;

public class GetAllOrderResponse
{
    public Guid OrderId { get; init; }
    
    public OrderState OrderState { get; init; }
    
    public DateTime OrderDate { get; init; }
}