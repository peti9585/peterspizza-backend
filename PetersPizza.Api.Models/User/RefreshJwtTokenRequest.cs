namespace PetersPizza.Api.Models.User;

public class RefreshJwtTokenRequest
{
    public Guid RefreshToken { get; init; }
}