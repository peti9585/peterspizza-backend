using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetersPizza.Api.Infrastructure.DataTransferObjects.Pizza;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Infrastructure.Repositories.Pizza;

// TODO: Align connection with some wrapper class
public class PizzaRepository(
    IConfiguration configuration,
    IMapper mapper) : IPizzaRepository
{
    public async Task<GetAllPizzaDetailsResponse> GetAllPizzasAsync()
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        
        conn.Open();
        var result = await conn.QueryAsync<DbPizza>(sql: Constants.GetAllPizzasSp,
            commandType: CommandType.StoredProcedure);
        conn.Close();

        var model = mapper.Map(result);

        return model;
    }

    public async Task<GetPizzasByIdsResponse> GetPizzasByIdsAsync(IEnumerable<int> pizzaIds)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = CreateIntIdsUdt(pizzaIds);
        
        conn.Open();
        var result = await conn.QueryAsync<DbPizzaById>(sql: Constants.GetPizzasByIdsSp,
            param: parameters,
            commandType: CommandType.StoredProcedure);
        conn.Close();

        var model = mapper.Map(result);

        return model;
    }
    
    public async Task InsertPizzaOrderAsync(OrderPizzasRequest request)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = CreateInsertOrderUdt(request);
        
        conn.Open();
        await conn.ExecuteAsync(sql: Constants.InsertPizzaOrderSp,
            param: parameters,
            commandType: CommandType.StoredProcedure);
        conn.Close();
    }

    public async Task<GetAllOrdersResponse> GetAllOrdersByIdAsync(int userId)
    {
        await using var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Int32);
        
        conn.Open();
        var result = await conn.QueryAsync<DbGetAllOrders>(sql: Constants.GetAllPizzaOrdersByIdSp,
            param: parameters,
            commandType: CommandType.StoredProcedure);
        conn.Close();

        var modelResponse = mapper.Map(result);
        
        return modelResponse;
    }

    private static DynamicParameters CreateIntIdsUdt(IEnumerable<int> ids)
    {
        var parameters = new DynamicParameters();
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));

        foreach (var id in ids)
        {
            table.Rows.Add(id);
        }
        
        parameters.Add("@Ids", table.AsTableValuedParameter());
        
        return parameters;
    }
    
    private static DynamicParameters CreateInsertOrderUdt(OrderPizzasRequest request)
    {
        var parameters = new DynamicParameters();
        var table = new DataTable();
        table.Columns.Add("PizzaId", typeof(int));
        table.Columns.Add("PizzaCount", typeof(int));

        foreach (var pizzas in request.OrderPizzaRequests)
        {
            table.Rows.Add(pizzas.PizzaId, pizzas.Quantity);
        }
        
        parameters.Add("@UserId", request.UserId);
        parameters.Add("@OrderId", request.OrderId);
        parameters.Add("@PizzaDetails", table.AsTableValuedParameter(Constants.InsertOrderUdt));
        
        return parameters;
    }
}