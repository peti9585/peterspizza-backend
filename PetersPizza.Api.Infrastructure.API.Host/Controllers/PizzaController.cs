using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;

namespace PetersPizza.Api.Infrastructure.API.Host.Controllers;

public class PizzaController() : CarterModule("api/pizza")
{
    private const string Tag = "Pizza";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/getall", async(
                IPizzaService pizzaService,
                IMapper mapper) =>
            {
                var response = await pizzaService.GetAllPizzasAsync();
                var responseViewModel = mapper.Map(response);
                
                return Results.Ok(responseViewModel);
            })
        .WithTags(Tag)
        .RequireAuthorization("User");
        
        app.MapPost("/getbyids", async(
                [FromBody]IEnumerable<int> pizzaIds,
                IPizzaService pizzaService,
                IMapper mapper) =>
        {
            var pizzaIdsList = pizzaIds.ToList();
            if (pizzaIdsList.Count == 0 || pizzaIdsList.Any(p => p <= 0))
            {
                return Results.BadRequest();
            }
            
            var result = await pizzaService.GetPizzasByIdsAsync(pizzaIdsList);
            var mappedResult = mapper.Map(result);
            
            return Results.Ok(mappedResult);
        })
        .WithTags(Tag)
        .RequireAuthorization("User");
    }
}