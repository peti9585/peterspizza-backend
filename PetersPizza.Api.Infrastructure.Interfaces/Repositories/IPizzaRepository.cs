using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Infrastructure.Interfaces.Repositories;

public interface IPizzaRepository
{
    Task<GetAllPizzaDetailsResponse> GetAllPizzasAsync();
}