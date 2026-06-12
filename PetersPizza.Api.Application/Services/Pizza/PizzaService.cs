using PetersPizza.Api.Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using PetersPizza.Api.Application.SignalR;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Application.Services.Pizza;

public class PizzaService(
    IWebHostEnvironment environment,
    IPizzaRepository pizzaRepository,
    IAdminService adminService,
    IImageHandlerService imageHandlerService,
    IHubContext<AdminOrdersHub> hubContext) : IPizzaService
{
    public async Task<GetAllPizzasResponse> GetAllPizzasAsync()
    {
        var getResponse = await pizzaRepository.GetAllPizzasAsync();

        var getAllPizzasResponseList = ConstructPizzaResponse(getResponse).ToList();
        
        return new GetAllPizzasResponse { GetAllPizzasResponses = getAllPizzasResponseList };
    }

    public async Task InsertPizzaOrderAsync(OrderPizzasRequest request)
    {
        await pizzaRepository.InsertPizzaOrderAsync(request);
        var ordersForToday = await adminService.GetAllOrdersAsync();
        
        await hubContext.Clients.All.SendAsync("ReceiveOrderFromUser", ordersForToday);
    }

    public Task<GetAllOrdersResponse> GetAllOrdersByIdAsync(int userId)
        => pizzaRepository.GetAllOrdersByIdAsync(userId);

    public Task<GetPizzasByIdsResponse> GetPizzasByIdsAsync(IEnumerable<int> pizzaIds)
        => pizzaRepository.GetPizzasByIdsAsync(pizzaIds);

    private IEnumerable<GetPizzaResponse> ConstructPizzaResponse(GetAllPizzaDetailsResponse response)
    {
        foreach (var pizza in response.GetAllPizzaDetailResponses)
        {
            var imageBytes = imageHandlerService.GetImageBytesByFileName(pizza.PizzaImageId.ToString());
            
            yield return new GetPizzaResponse
            {
                PizzaId = pizza.PizzaId,
                PizzaName = pizza.PizzaName,
                Description = pizza.Description,
                PizzaImageBytes = imageBytes
            };
        }
    }
}