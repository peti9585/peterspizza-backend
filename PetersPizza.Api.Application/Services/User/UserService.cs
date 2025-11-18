using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Application.Services.User;

public class UserService(
    IUserRepository userRepository,
    IConfiguration configuration,
    PasswordHasher<RegisterUserRequest> passwordHasher,
    JwtSecurityTokenHandler jwtTokenHandler) : IUserService
{
    public async Task<int> RegisterUserAsync(RegisterUserRequest request)
    {
        var hashedPassword = passwordHasher.HashPassword(request, request.Password);
        var requestWithHashedPassword = new RegisterUserRequest
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = hashedPassword
        };

        var userId = await userRepository.RegisterUserAsync(requestWithHashedPassword);

        return userId;
    }

    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
    {
        var repositoryResponse = await userRepository.LoginUserAsync(request);

        if (!IsValidPassword(request.Password, repositoryResponse.PasswordHash)) return new LoginUserResponse();

        var refreshToken = await UpsertRefreshTokenAsync(repositoryResponse.UserId);

        return new LoginUserResponse
        {
            Name = repositoryResponse.Name,
            JwtToken = GenerateJwtTokenForUser(request.UserName, repositoryResponse.UserId),
            RefreshToken = refreshToken
        };
    }

    public async Task<RefreshJwtTokenResponse> RefreshJwtTokenAsync(RefreshJwtTokenRequest request)
    {
        var getUserInfo = await userRepository.GetUserByRefreshTokenAsync(request.RefreshToken);
        
        if (getUserInfo.Id is 0) return new RefreshJwtTokenResponse();
        
        var refreshToken = await UpsertRefreshTokenAsync(getUserInfo.Id);

        return new RefreshJwtTokenResponse
        {
            JwtToken = GenerateJwtTokenForUser(getUserInfo.UserName, getUserInfo.Id),
            RefreshToken = refreshToken
        };
    }

    public Task<GetUserDetailsByIdResponse> GetUserByIdAsync(int userId)
        => userRepository.GetUserByIdAsync(userId);

    private async Task<Guid> UpsertRefreshTokenAsync(int userId)
    {
        var refreshToken = Guid.NewGuid();
        await userRepository.UpsertRefreshTokenAsync(userId, refreshToken);
        
        return refreshToken;
    }

    private bool IsValidPassword(string providedPassword, string storedPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(storedPasswordHash)) return false;
        
        var result = passwordHasher.VerifyHashedPassword(null, storedPasswordHash, providedPassword);
        
        return result == PasswordVerificationResult.Success;
    }

    private string GenerateJwtTokenForUser(string userName, int userId)
    {
        var key = configuration.GetSection(Constants.JwtKey).Value;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, Constants.UserRole),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            ]),
            Issuer = Constants.ApplicationName,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var temp = jwtTokenHandler.WriteToken(token);
        
        return temp;
    }
}