using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Infrastructure.Interfaces.Repositories;

public interface IPizzaRepository
{
    Task<GetAllPizzaDetailsResponse> GetAllPizzasAsync();
    Task<GetPizzasByIdsResponse> GetPizzasByIdsAsync(IEnumerable<int> pizzaIds);
    Task InsertPizzaOrderAsync(OrderPizzasRequest request);
}