using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Application.Interfaces.SignalR;

public interface IAdminOrdersHub
{
    Task SendNewOrderNotificationToAdmin(GetAllOrdersResponse orders);
}