namespace PetersPizza.Api.Application.Interfaces.Services;

public interface IPasswordHandlerService<in T> where T : class
{
    string HashPassword(T user, string password);
    bool IsValidPassword(string providedPassword, string storedPasswordHash);
}