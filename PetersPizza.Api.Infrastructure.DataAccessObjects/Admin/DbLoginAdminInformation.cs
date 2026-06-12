namespace PetersPizza.Api.Infrastructure.DataTransferObjects.Admin;

public class DbLoginAdminInformation
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public string Password { get; init; }
}