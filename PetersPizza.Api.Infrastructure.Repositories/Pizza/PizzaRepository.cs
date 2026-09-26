using Microsoft.EntityFrameworkCore;
using PetersPizza.Api.Infrastructure.EntityFramework;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Infrastructure.Repositories.Pizza;

public class PizzaRepository(AppDbContext dbContext) : IPizzaRepository
{
    public async Task<GetAllPizzaDetailsResponse> GetAllPizzasAsync()
    {
        var pizzas = await dbContext.Pizza
            .Select(p => new GetAllPizzaDetailResponse
            {
                PizzaId = p.Id,
                PizzaName = p.Name,
                Description = p.Description,
                PizzaImageId = p.ImageId
            })
            .AsNoTracking()
            .ToListAsync();

        return new GetAllPizzaDetailsResponse { GetAllPizzaDetailResponses = pizzas };
    }

    public async Task<GetPizzasByIdsResponse> GetPizzasByIdsAsync(IEnumerable<int> pizzaIds)
    {
        var pizzas = await dbContext.Pizza
            .Where(p => pizzaIds.Contains(p.Id))
            .Select(p => new GetPizzaByIdResponse
            {
                PizzaId = p.Id,
                PizzaName = p.Name,
                PizzaPrice = p.Price
            })
            .AsNoTracking()
            .ToListAsync();

        return new GetPizzasByIdsResponse { GetPizzaResponses = pizzas };
    }
    
    public async Task InsertPizzaOrderAsync(OrderPizzasRequest request)
    {
        var orderEntities = request.OrderPizzaRequests.Select(p => new Models.Entities.Order
        {
            UserId = request.UserId,
            OrderId = request.OrderId,
            PizzaId = p.PizzaId,
            Count = p.Quantity,
            OrderStateId = (int)Models.Common.OrderState.WaitingToAccept,
            OrderDate = DateTime.UtcNow
        }).ToList();
        
        dbContext.Order.AddRange(orderEntities);
        await dbContext.SaveChangesAsync();
    }

    public async Task<GetAllOrdersResponse> GetAllOrdersByIdAsync(int userId)
    {
        var orders = await dbContext.Order
            .Where(x => x.UserId == userId)
            .Select(x => new GetAllOrderResponse
            {
                OrderId = x.OrderId,
                OrderState = (Models.Common.OrderState)x.OrderStateId,
                OrderDate = x.OrderDate
            })
            .AsNoTracking()
            .ToListAsync();
        
        var distinctOrders = orders.GroupBy(o => o.OrderId)
            .Select(g => g.First())
            .ToList();
        
        return new GetAllOrdersResponse { GetAllOrderResponses = distinctOrders };
    }
}