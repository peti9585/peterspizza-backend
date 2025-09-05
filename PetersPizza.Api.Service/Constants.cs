using System.Diagnostics.CodeAnalysis;

namespace PetersPizza.Api.Service;

[ExcludeFromCodeCoverage]
internal static class Constants
{
    // Roles and Policies
    internal const string User = "User";

    internal const string DefaultCorsPolicy = "DefaultPolicy";
    
    // Application
    internal const string ApplicationName = "PetersPizza.Api";
    internal const string JwtKey = "Jwt:Key";
}