using Microsoft.AspNetCore.Identity;
using PetersPizza.Api.Application.Interfaces.Services;

namespace PetersPizza.Api.Application.Services.Common;

public class PasswordHandlerService<T>(PasswordHasher<T> passwordHasher) : IPasswordHandlerService<T> where T : class
{
    public string HashPassword(T user, string password)
        => passwordHasher.HashPassword(user, password);

    public bool IsValidPassword(string providedPassword, string storedPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(storedPasswordHash)) return false;
        
        var result = passwordHasher.VerifyHashedPassword(null, storedPasswordHash, providedPassword);
        
        return result == PasswordVerificationResult.Success;
    }
}