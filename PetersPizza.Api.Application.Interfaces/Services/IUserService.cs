using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Application.Interfaces.Services;

public interface IUserService
{
    public Task<int> RegisterUserAsync(RegisterUserRequest request);
    public Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request);
    public Task<RefreshJwtTokenResponse> RefreshJwtTokenAsync(RefreshJwtTokenRequest request);
    public Task<GetUserDetailsByIdResponse> GetUserByIdAsync(int userId);
}