using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PetersPizza.Api.Application.Interfaces.SignalR;
using PetersPizza.Api.Models.SignalR;

namespace PetersPizza.Api.Application.SignalR;

[Authorize(Roles = "User")]
public class UserOrdersHub : Hub, IUserOrdersHub
{
    public async Task SendOrderStatusUpdateToUser(string userId, OrderStatusChangedNotification status)
    {
        await Clients.User(userId).SendAsync("ReceiveOrderStatus", status);
    }
}