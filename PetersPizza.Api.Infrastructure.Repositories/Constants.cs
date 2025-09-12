using System.Diagnostics.CodeAnalysis;

namespace PetersPizza.Api.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
internal static class Constants
{
    // Stored Procedures
    internal const string InsertUserSp = "[dbo].[InsertUser]";
    internal const string GetUserSp = "[dbo].[GetUser]";
    internal const string UpsertRefreshTokenSp = "[dbo].[UpsertRefreshToken]";
    internal const string GetUserByRefreshTokenSp = "[dbo].[GetUserByRefreshToken]";
    internal const string InsertPizzaSp = "[dbo].[InsertPizza]";
    internal const string GetAllPizzasSp = "[dbo].[GetAllPizzas]";
}