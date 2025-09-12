namespace PetersPizza.Api.Infrastructure.DataTransferObjects.Pizza;

public class DbPizza
{
    public string Name { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public Guid ImageId { get; init; }
}