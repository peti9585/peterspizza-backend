using Microsoft.AspNetCore.Hosting;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Application.Services.Pizza;

public class PizzaService(
    IWebHostEnvironment environment,
    IPizzaRepository pizzaRepository) : IPizzaService
{
    public async Task<GetAllPizzasResponse> GetAllPizzasAsync()
    {
        var getResponse = await pizzaRepository.GetAllPizzasAsync();

        var getAllPizzasResponseList = GetPizzaResponse(getResponse).ToList();
        
        return new GetAllPizzasResponse { GetAllPizzasResponses = getAllPizzasResponseList };
    }

    public Task InsertPizzaOrderAsync(OrderPizzasRequest request)
        => pizzaRepository.InsertPizzaOrderAsync(request);

    public Task<GetPizzasByIdsResponse> GetPizzasByIdsAsync(IEnumerable<int> pizzaIds)
        => pizzaRepository.GetPizzasByIdsAsync(pizzaIds);

    private IEnumerable<GetPizzaResponse> GetPizzaResponse(GetAllPizzaDetailsResponse response)
    {
        foreach (var pizza in response.GetAllPizzaDetailResponses)
        {
            var imagePath = Path.Combine(environment.WebRootPath, "images", pizza.PizzaImageId.ToString());
            
            var imageBytes = File.ReadAllBytes(imagePath);
            
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