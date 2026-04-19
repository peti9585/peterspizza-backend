using System.Diagnostics.CodeAnalysis;

namespace PetersPizza.Api.Application;

[ExcludeFromCodeCoverage]
internal static class Constants
{
    // Roles
    internal const string UserRole = "User";
    internal const string AdminRole = "Admin";
    
    // Application
    internal const string ApplicationName = "PetersPizza.Api";
    internal const string JwtKey = "Jwt:Key";
}