using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Infrastructure.Interfaces.Repositories;

public interface IAdminRepository
{
    Task InsertPizzaAsync(InsertPizzaRequest request);
}