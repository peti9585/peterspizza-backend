namespace PetersPizza.Api.Models.User;

public class LoginUserInformation
{
    public int UserId { get; init; }
    
    public string Name { get; init; }
    
    public string PasswordHash { get; init; }
}