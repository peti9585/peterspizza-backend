using Microsoft.EntityFrameworkCore;
using PetersPizza.Api.Infrastructure.EntityFramework;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Entities;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Repositories.User;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<int> RegisterUserAsync(RegisterUserRequest request)
    {
        var user = new Models.Entities.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password
        };
        
        dbContext.User.Add(user);
        await dbContext.SaveChangesAsync();
        
        return user.Id;
    }

    public async Task<LoginUserInformation> GetUserDetailsAsync(LoginUserRequest request)
    {
        return await dbContext.User
            .Where(u => u.UserName == request.UserName)
            .Select(u => new LoginUserInformation
            {
                UserId = u.Id,
                Name = u.UserName,
                PasswordHash = u.Password
            })
            .AsNoTracking()
            .SingleOrDefaultAsync() ?? new LoginUserInformation();
    }

    public async Task UpsertRefreshTokenAsync(int userId, Guid refreshToken)
    {
        var newExpiryDate = DateTime.UtcNow.AddDays(1);
        
        var existingToken = await dbContext.UserRefreshToken
            .SingleOrDefaultAsync(x => x.UserId == userId);
        
        if (existingToken != null)
        {
            await dbContext.UserRefreshToken
                .Where(u => u.UserId == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.RefreshToken, refreshToken)
                    .SetProperty(t => t.ExpirationDate, newExpiryDate));
        }
        else
        {
            dbContext.UserRefreshToken.Add(new UserRefreshToken
            {
                UserId = userId,
                RefreshToken = refreshToken,
                ExpirationDate = newExpiryDate
            });
            
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<UserInfo> GetUserByRefreshTokenAsync(Guid refreshToken)
    {
        return await dbContext.UserRefreshToken
            .Where(t => t.RefreshToken == refreshToken && t.ExpirationDate > DateTime.UtcNow)
            .Join(dbContext.User,
                token => token.UserId,
                user => user.Id,
                (token, user) => new UserInfo
                {
                    Id = user.Id,
                    UserName = user.UserName
                })
            .AsNoTracking()
            .SingleOrDefaultAsync() ?? new UserInfo();
    }

    public async Task<GetUserDetailsByIdResponse> GetUserByIdAsync(int userId)
    {
        return await dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => new GetUserDetailsByIdResponse
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            })
            .AsNoTracking()
            .SingleOrDefaultAsync() ?? new GetUserDetailsByIdResponse();
    }

    public async Task<bool> AreUserValuesUniqueAsync(string phoneNumber, string email, int userId)
    {
        return !await dbContext.User
            .AsNoTracking()
            .AnyAsync(u => (u.PhoneNumber.Equals(phoneNumber) || u.Email.Equals(email)) && u.Id != userId);
    }

    public async Task UpdateUserAsync(UpdateUserRequest request, int userId)
    {
        await dbContext.User
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.FirstName, u => request.FirstName)
                .SetProperty(u => u.LastName, u => request.LastName)
                .SetProperty(u => u.PhoneNumber, u => request.PhoneNumber)
                .SetProperty(u => u.Email, u => request.Email));
    }
}