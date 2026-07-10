using PetersPizza.Api.Models.SignalR;

namespace PetersPizza.Api.Application.Interfaces.SignalR;

public interface IUserOrdersHub
{
    Task SendOrderStatusUpdateToUser(string userId, OrderStatusChangedNotification status);
}