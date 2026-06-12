namespace PetersPizza.Api.Models.Admin;

public class LoginAdminRequest
{
    public string UserName { get; init; }
    
    public string Password { get; init; }
}