namespace PetersPizza.Api.Models.User;

public class LoginUserResponse
{
    public string Name { get; init; }
    
    public string JwtToken { get; init; }
}