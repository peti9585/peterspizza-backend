using System.Diagnostics.CodeAnalysis;

namespace PetersPizza.Api.Application;

[ExcludeFromCodeCoverage]
internal static class Constants
{
    // Roles
    internal const string UserRole = "User";
    
    // Application
    internal const string ApplicationName = "PetersPizza.Api";
    internal const string JwtKey = "Jwt:Key";
}