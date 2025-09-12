using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetersPizza.Api.Infrastructure.DataTransferObjects.User;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Repositories.User;

public class UserRepository(IConfiguration configuration,
    IMapper mapper) : IUserRepository
{
    public async Task<int> RegisterUserAsync(RegisterUserRequest request)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = CreateRegisterUserParameters(request);

        conn.Open();
        var userId = await conn.QuerySingleOrDefaultAsync<int>(sql: Constants.InsertUserSp, 
            param: parameters, 
            commandType: CommandType.StoredProcedure);
        conn.Close();

        return userId;
    }

    public async Task<LoginUserInformation> LoginUserAsync(LoginUserRequest request)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = CreateLoginUserParameters(request);
        
        conn.Open();
        var result = await conn.QuerySingleOrDefaultAsync<DbLoginUserInformation>(sql: Constants.GetUserSp, 
            param: parameters, 
            commandType: CommandType.StoredProcedure);
        conn.Close();
        
        return result is null ? new LoginUserInformation() : mapper.Map(result);
    }

    public async Task UpsertRefreshTokenAsync(int userId, Guid refreshToken)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = CreateUpsertRefreshTokenParameters(userId, refreshToken);
        
        conn.Open();
        await conn.ExecuteAsync(sql: Constants.UpsertRefreshTokenSp,
            param: parameters, 
            commandType: CommandType.StoredProcedure);
        conn.Close();
    }

    public async Task<UserInfo> GetUserByRefreshTokenAsync(Guid refreshToken)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = new DynamicParameters();
        parameters.Add("@RefreshToken", refreshToken, DbType.Guid);
        
        conn.Open();
        var result = await conn.QuerySingleOrDefaultAsync<DbUserInfo>(sql: Constants.GetUserByRefreshTokenSp,
            param: parameters, 
            commandType: CommandType.StoredProcedure);
        conn.Close();
        
        return result is null ? new UserInfo() : mapper.Map(result);
    }

    private static DynamicParameters CreateUpsertRefreshTokenParameters(int userId, Guid refreshToken)
    {
        var parameters = new DynamicParameters();
        
        parameters.Add("@UserId", userId, DbType.Int32);
        parameters.Add("@RefreshToken", refreshToken, DbType.Guid);
        
        return parameters;
    }

    private static DynamicParameters CreateLoginUserParameters(LoginUserRequest request)
    {
        var parameters = new DynamicParameters();

        parameters.Add("UserName", request.UserName, DbType.String);

        return parameters;
    }

    private static DynamicParameters CreateRegisterUserParameters(RegisterUserRequest request)
    {
        var parameters = new DynamicParameters();

        parameters.Add("FirstName", request.FirstName, DbType.String);
        parameters.Add("LastName", request.LastName, DbType.String);
        parameters.Add("UserName", request.UserName, DbType.String);
        parameters.Add("Email", request.Email, DbType.String);
        parameters.Add("PhoneNumber", request.PhoneNumber, DbType.String);
        parameters.Add("PasswordHash", request.Password, DbType.String);

        return parameters;
    }
}