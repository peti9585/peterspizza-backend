namespace PetersPizza.Api.Models.User;

public class LoginUserRequest
{
    public string UserName { get; init; }
    
    public string Password { get; init; }
}