using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Application.Interfaces.Services;

public interface IPizzaService
{
    Task<GetAllPizzasResponse> GetAllPizzasAsync();

    Task<GetPizzasByIdsResponse> GetPizzasByIdsAsync(IEnumerable<int> pizzaIds);

    Task InsertPizzaOrderAsync(OrderPizzasRequest request);
}