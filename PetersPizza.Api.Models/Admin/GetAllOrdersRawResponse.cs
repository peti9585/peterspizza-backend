using PetersPizza.Api.Models.Common;

namespace PetersPizza.Api.Models.Admin;

public class GetAllOrdersRawResponse
{
    public int OrderIdInteger { get; init; }
    
    public Guid OrderIdGuid { get; init; }
    
    public string UserName { get; init; }
    
    public string PizzaName { get; init; }
    
    public int Quantity { get; init; }
    
    public decimal Price { get; init; }
    
    public OrderState OrderState { get; init; }
    
    public DateTime OrderDate { get; init; }
}