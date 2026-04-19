namespace PetersPizza.Api.Infrastructure.DataTransferObjects.Pizza;

public class DbGetAllOrders
{
    public Guid OrderId { get; init; }
    
    public int OrderState { get; init; }
    
    public DateTime OrderDate { get; init; }
}