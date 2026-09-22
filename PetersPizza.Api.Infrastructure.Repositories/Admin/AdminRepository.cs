using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetersPizza.Api.Infrastructure.DataAccessObjects.Admin;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Infrastructure.Repositories.Admin;

public class AdminRepository(
    IConfiguration configuration,
    IMapper mapper) : IAdminRepository
{
    public async Task InsertPizzaAsync(InsertPizzaRequest request)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = CreateInsertPizzaParameters(request);
        
        conn.Open();
        await conn.ExecuteAsync(sql: Constants.InsertPizzaSp,
            param: parameters, 
            commandType: CommandType.StoredProcedure);
        conn.Close();
    }

    public async Task<LoginAdminInformation> LoginAdminAsync(LoginAdminRequest request)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = new DynamicParameters();
        parameters.Add("@UserName", request.UserName, DbType.String);
        
        conn.Open();
        var result = await conn.QuerySingleOrDefaultAsync<DbLoginAdminInformation>(sql: Constants.GetAdminSp,
            param: parameters, 
            commandType: CommandType.StoredProcedure);
        conn.Close();
        
        return result is null ? new LoginAdminInformation() : mapper.Map(result);
    }

    public async Task<IEnumerable<GetAllOrdersRawResponse>> GetAllOrdersForTodayAsync()
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        
        conn.Open();
        var result = await conn.QueryAsync<DbGetAllOrdersForToday>(sql: Constants.GetAllOrdersForTodaySp,
            commandType: CommandType.StoredProcedure);
        conn.Close();
        
        var response = mapper.Map(result);
        
        return response;
    }

    public async Task<int> ChangeOrderStateAsync(ChangeOrderStateRequest request)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", request.OrderId, DbType.Guid);
        parameters.Add("@NewOrderState", request.NewOrderState, DbType.Int32);
        
        conn.Open();
        var userId = await conn.QueryFirstOrDefaultAsync<int>(sql: Constants.ChangeOrderStateSp,
            param: parameters, 
            commandType: CommandType.StoredProcedure);
        conn.Close();

        return userId;
    }

    private static DynamicParameters CreateInsertPizzaParameters(InsertPizzaRequest request)
    {
        var parameters = new DynamicParameters();
        
        parameters.Add("@Name", request.PizzaName, DbType.String);
        parameters.Add("@Description", request.Description, DbType.String);
        parameters.Add("@Price", request.PizzaPrice, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@ImageId", request.PizzaImageId, DbType.Guid);
        
        return parameters;
    }
}