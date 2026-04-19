using PetersPizza.Api.Models.Common;

namespace PetersPizza.Api.Models.SignalR;

public class OrderStatusChangedNotification
{
    public Guid OrderId { get; init; }
    
    public OrderState NewOrderState { get; init; }
}