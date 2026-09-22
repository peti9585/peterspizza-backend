namespace PetersPizza.Api.Infrastructure.DataAccessObjects.User;

public class DbGetUserDetailsByIdResponse
{
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
    
    public string PhoneNumber { get; init; }
    
    public string Email { get; init; }
}