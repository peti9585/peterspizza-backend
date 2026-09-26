using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<int> RegisterUserAsync(RegisterUserRequest request);
    public Task<LoginUserInformation> GetUserDetailsAsync(LoginUserRequest request);
    public Task UpsertRefreshTokenAsync(int userId, Guid refreshToken);
    public Task<UserInfo> GetUserByRefreshTokenAsync(Guid refreshToken);
    public Task<GetUserDetailsByIdResponse> GetUserByIdAsync(int userId);
    public Task<bool> AreUserValuesUniqueAsync(string phoneNumber, string email, int userId);
    public Task UpdateUserAsync(UpdateUserRequest request, int userId);
}