namespace PetersPizza.Api.ViewModels.User;

public class RefreshJwtTokenResponse
{
    public string JwtToken { get; init; } = string.Empty;
    
    public Guid RefreshToken { get; init; }
}