namespace PetersPizza.Api.Infrastructure.DataTransferObjects.User;

public class DbGetUserDetailsByIdResponse
{
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
    
    public string PhoneNumber { get; init; }
    
    public string Email { get; init; }
}