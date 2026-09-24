using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Infrastructure.Interfaces.Repositories;

public interface IAdminRepository
{
    Task InsertPizzaAsync(InsertPizzaRequest request);
    Task<LoginAdminInformation> GetAdminDetailsAsync(LoginAdminRequest request);
    Task<IEnumerable<GetAllOrdersRawResponse>> GetAllOrdersForTodayAsync();
    Task<int> ChangeOrderStateAsync(ChangeOrderStateRequest request);
}