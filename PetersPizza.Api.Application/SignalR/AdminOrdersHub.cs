using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PetersPizza.Api.Application.Interfaces.SignalR;
using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Application.SignalR;

[Authorize(Roles = "Admin")]
public class AdminOrdersHub(IHubContext<AdminOrdersHub> hubContext) : Hub, IAdminOrdersHub
{
    public async Task SendNewOrderNotificationToAdmin(GetAllOrdersResponse orders)
    {
        await hubContext.Clients.All.SendAsync("ReceiveOrderFromUser", orders);
    }
}