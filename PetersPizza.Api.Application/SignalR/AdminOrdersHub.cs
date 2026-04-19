using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PetersPizza.Api.Application.SignalR;

[Authorize(Roles = "Admin")]
public class AdminOrdersHub : Hub;