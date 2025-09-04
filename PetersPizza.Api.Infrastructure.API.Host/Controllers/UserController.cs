using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.ViewModels.User;

namespace PetersPizza.Api.Infrastructure.API.Host.Controllers;

public class UserController() : CarterModule("api/user")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // User Registration
        app.MapPost<RegisterUserRequest>("register", async (
            [FromBody]RegisterUserRequest request, 
            IValidator<RegisterUserRequest> validator,
            IMapper mapper,
            IUserService userService) =>
        {
            validator.ValidateAndThrow(request);
            var requestModel = mapper.Map(request);

            var userId = await userService.RegisterUserAsync(requestModel);

            return userId > 0 
                ? Results.Ok()
                : Results.Conflict();
        });
        
        // User Login
        app.MapPost<LoginUserRequest>("login", async (
            [FromBody]LoginUserRequest request,
            HttpContext context,
            IValidator<LoginUserRequest> validator,
            IMapper mapper,
            IUserService userService) =>
        {
            validator.ValidateAndThrow(request);
            var requestModel = mapper.Map(request);

            var response = await userService.LoginUserAsync(requestModel);
            
            if (string.IsNullOrEmpty(response.JwtToken) || string.IsNullOrWhiteSpace(response.Name))
            {
                return Results.NotFound();
            }
            
            context.Response.Cookies.Append("jwt", response.JwtToken, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true, TODO: Enable in production,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            
            return Results.Ok(response.Name);
        });
    }
}