using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<int> RegisterUserAsync(RegisterUserRequest request);
    
    public Task<LoginUserInformation> LoginUserAsync(LoginUserRequest request);
}