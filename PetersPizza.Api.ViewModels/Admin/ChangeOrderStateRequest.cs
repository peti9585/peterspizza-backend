using PetersPizza.Api.ViewModels.Common;

namespace PetersPizza.Api.ViewModels.Admin;

public class ChangeOrderStateRequest
{
    public Guid OrderId { get; init; }
    
    public OrderState NewOrderState { get; init; }
}