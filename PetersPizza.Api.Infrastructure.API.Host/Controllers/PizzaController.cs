using System.Security.Claims;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.ViewModels.Pizza;

namespace PetersPizza.Api.Infrastructure.API.Host.Controllers;

public class PizzaController() : CarterModule("api/pizza")
{
    private const string Tag = "Pizza";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/getall", async (
                IPizzaService pizzaService,
                IMapper mapper) =>
            {
                var response = await pizzaService.GetAllPizzasAsync();
                var responseViewModel = mapper.Map(response);
                
                return Results.Ok(responseViewModel);
            })
        .WithTags(Tag)
        .RequireAuthorization("User");
        
        app.MapPost("/getbyids", async (
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
        
        app.MapPost("/order", async (
                HttpContext context,
                [FromBody]OrderPizzasRequest request,
                IValidator<OrderPizzasRequest> validator,
                IPizzaService pizzaService,
                IMapper mapper) =>
        {
            var userIdString = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdString)) return Results.BadRequest("Invalid credentials.");
                
            int.TryParse(userIdString, out var userId);

            var finalRequest = new OrderPizzasRequest
            {
                UserId = userId,
                OrderId = request.OrderId,
                OrderPizzaRequests = request.OrderPizzaRequests
            };
            
            validator.ValidateAndThrow(finalRequest);
            var requestModel = mapper.Map(finalRequest);

            await pizzaService.InsertPizzaOrderAsync(requestModel);
            
            return Results.Ok();
        })
        .WithTags(Tag)
        .RequireAuthorization("User");

        app.MapGet("/orders-all", async (
                IPizzaService pizzaService,
                IMapper mapper,
                HttpContext context) =>
            {
                int.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
                
                if (userId <= 0) return Results.BadRequest("Invalid user ID.");
                
                var result = await pizzaService.GetAllOrdersByIdAsync(userId);
                var resultViewModel = mapper.Map(result);
                
                return Results.Ok(resultViewModel);
            })
            .WithTags(Tag)
            .RequireAuthorization("User");
    }
}