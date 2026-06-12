using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PetersPizza.Api.Application.SignalR;

[Authorize(Roles = "User")]
public class UserOrdersHub : Hub;