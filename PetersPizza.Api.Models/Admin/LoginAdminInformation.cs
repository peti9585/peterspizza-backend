namespace PetersPizza.Api.Models.Admin;

public class LoginAdminInformation
{
    public int AdminId { get; init; }
    
    public string Name { get; init; }
    
    public string PasswordHash { get; init; }
}