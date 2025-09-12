using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Infrastructure.Repositories.Admin;

public class AdminRepository(IConfiguration configuration) : IAdminRepository
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
    
    private static DynamicParameters CreateInsertPizzaParameters(InsertPizzaRequest request)
    {
        var parameters = new DynamicParameters();
        
        parameters.Add("@Name", request.PizzaName, DbType.String);
        parameters.Add("@Description", request.Description, DbType.String);
        parameters.Add("@ImageId", request.PizzaImageId, DbType.Guid);
        
        return parameters;
    }
}