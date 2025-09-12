using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetersPizza.Api.Infrastructure.DataTransferObjects.Pizza;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Pizza;

namespace PetersPizza.Api.Infrastructure.Repositories.Pizza;

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
}