using PetersPizza.Api.Models.Common;

namespace PetersPizza.Api.Models.Admin;

public class GetAllOrderResponse
{
    public Guid OrderId { get; init; }
    
    public string UserName { get; init; }
    
    public OrderState OrderState { get; init; }
    
    public DateTime OrderDate { get; init; }
    
    public IEnumerable<OrderItem> OrderItems { get; init; }
}