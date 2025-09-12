namespace PetersPizza.Api.Infrastructure.DataTransferObjects.User;

public class DbUserInfo
{
    public int Id { get; init; }
    
    public string UserName { get; init; } = string.Empty;
}