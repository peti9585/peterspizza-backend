namespace PetersPizza.Api.ViewModels.User;

public class LoginUserResponse
{
    public string Name { get; init; } = string.Empty;

    public string JwtToken { get; init; } = string.Empty;
    
    public Guid RefreshToken { get; init; }
}