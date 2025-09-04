namespace PetersPizza.Api.Models.User;

public class LoginUserInformation
{
    public string Name { get; init; }
    
    public string PasswordHash { get; init; }
}