using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Application.Interfaces.Services;

public interface IAdminService
{
    Task UploadPizzaAsync(UploadPizzaRequest request);
    Task<LoginAdminResponse> LoginAdminAsync(LoginAdminRequest request);
    Task<GetAllOrdersResponse> GetAllOrdersAsync();
    Task ChangeOrderStateAsync(ChangeOrderStateRequest request);
    
    JwtTokenInformationResponse ExtractJwtInformationFromToken(JwtTokenInformationRequest request);
}