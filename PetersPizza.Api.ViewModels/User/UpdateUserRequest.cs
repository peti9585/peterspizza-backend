namespace PetersPizza.Api.ViewModels.User;

public class UpdateUserRequest
{
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
    
    public string PhoneNumber { get; init; }
    
    public string Email { get; init; }
}