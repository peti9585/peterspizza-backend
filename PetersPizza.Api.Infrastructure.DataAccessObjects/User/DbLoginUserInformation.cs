namespace PetersPizza.Api.Infrastructure.DataTransferObjects.User;

public class DbLoginUserInformation
{
    public int Id { get; init; }
    
    public string FirstName { get; init; }
    
    public string Password { get; init; }
}