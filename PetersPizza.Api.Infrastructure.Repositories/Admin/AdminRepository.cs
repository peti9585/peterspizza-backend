using Microsoft.EntityFrameworkCore;
using PetersPizza.Api.Infrastructure.EntityFramework;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.Common;

namespace PetersPizza.Api.Infrastructure.Repositories.Admin;

public class AdminRepository(AppDbContext dbContext) : IAdminRepository
{
    public async Task InsertPizzaAsync(InsertPizzaRequest request)
    {
        var pizza = new Models.Entities.Pizza
        {
            Name = request.PizzaName,
            Description = request.Description,
            Price = request.PizzaPrice,
            ImageId = request.PizzaImageId
        };

        dbContext.Pizza.Add(pizza);

        await dbContext.SaveChangesAsync();
    }

    public async Task<LoginAdminInformation> GetAdminDetailsAsync(LoginAdminRequest request)
    {
        return await dbContext.Admin
            .Where(a => a.Name == request.UserName)
            .Select(a => new LoginAdminInformation
            {
                AdminId = a.Id,
                Name = a.Name,
                PasswordHash = a.Password
            })
            .AsNoTracking()
            .SingleOrDefaultAsync() ?? new LoginAdminInformation();
    }

    public async Task<IEnumerable<GetAllOrdersRawResponse>> GetAllOrdersForTodayAsync()
    {
        return await dbContext.Order
            .Join(dbContext.Pizza,
                o => o.PizzaId,
                p => p.Id,
                (order, pizza) => new { order, pizza })
            .Join(dbContext.User,
                combined => combined.order.UserId,
                u => u.Id,
                (combined, user) => new { combined.order, combined.pizza, user })
            .Where(x => x.order.OrderDate.Date == DateTime.UtcNow.Date)
            .Select(x => new GetAllOrdersRawResponse
            {
                OrderIdInteger = x.order.Id,
                OrderIdGuid = x.order.OrderId,
                UserName = x.user.FirstName + " " + x.user.LastName,
                PizzaName = x.pizza.Name,
                Price = x.pizza.Price,
                Quantity = x.order.Count,
                OrderState = (OrderState)x.order.OrderStateId,
                OrderDate = x.order.OrderDate
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> ChangeOrderStateAsync(ChangeOrderStateRequest request)
    {
        var userId = await dbContext.Order
            .Where(o => o.OrderId == request.OrderId)
            .AsNoTracking()
            .Select(o => o.UserId)
            .FirstOrDefaultAsync();
        
        await dbContext.Order
            .Where(o => o.OrderId == request.OrderId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(o => o.OrderStateId, (int)request.NewOrderState));

        return userId;
    }
}