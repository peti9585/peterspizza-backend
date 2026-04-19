using PetersPizza.Api.Models.Common;

namespace PetersPizza.Api.Models.Admin;

public class ChangeOrderStateRequest
{
    public Guid OrderId { get; init; }
    
    public OrderState NewOrderState { get; init; }
}