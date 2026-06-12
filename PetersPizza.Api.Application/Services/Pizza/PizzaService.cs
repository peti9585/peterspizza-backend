using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Application.Services.Pizza;

public class PizzaService(IImageHandlerService imageHandlerService, IPizzaRepository pizzaRepository) : IPizzaService
{
    public async Task<GetAllPizzasResponse> GetAllPizzasAsync()
    {
        var getResponse = await pizzaRepository.GetAllPizzasAsync();

        var getAllPizzasResponseList = ConstructPizzaResponse(getResponse).ToList();
        
        return new GetAllPizzasResponse { GetAllPizzasResponses = getAllPizzasResponseList };
    }

    public Task InsertPizzaOrderAsync(OrderPizzasRequest request)
        => pizzaRepository.InsertPizzaOrderAsync(request);

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